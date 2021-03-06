using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Application.Interfaces;
using Domain.Entities;
using Domain.ResourceParameters;
using Domain;
using System.Security.Claims;
using System;
using Domain.Enums;
using Infrastructure.Interfaces;
using Application.Controllers.ControllerModels;
using System.Linq;
using Application.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBaseWithUri<BoardsResourceParameters>
    {
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;
        private readonly IMetadataHelper<Board> _metadataHelper;

        public BoardsController(IBoardService boardService, IMapper mapper, IMetadataHelper<Board> metadataHelper)
        {
            _mapper = mapper;
            _metadataHelper = metadataHelper;
            _boardService = boardService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBoardAsync(BoardForCreationDto boardForCreationDto)
        {
            var board = _mapper.Map<Board>(boardForCreationDto);

            // Przypisanie UserId z tokenu
            board.UserId = Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var tempVal) ? tempVal : (int?)null;

            var boardFromRepo = await _boardService.AddBoardAsync(board);

            if (!await _boardService.SaveChangesAsync())
                return BadRequest();

            var boardToReturn = _mapper.Map<BoardForListDto>(boardFromRepo);

            return CreatedAtRoute("GetBoardAsync", new { controller = "Boards", id = boardFromRepo.Id }, boardToReturn);
        }

        [HttpGet("{id}", Name = nameof(GetBoardAsync))]
        public async Task<IActionResult> GetBoardAsync(int id)
        {
            var board = await _boardService.GetBoardAsync(id);

            if (board == null)
                return NotFound();

            if (IsOwner(board.UserId) || await IsViewer(id) || HttpContext.User.IsInRole(UserRoles.Admin) || HttpContext.User.IsInRole(UserRoles.Mod))
            {
                var boardToReturn = _mapper.Map<BoardForListDto>(board);
                return Ok(boardToReturn);
            }

            return Unauthorized("Insufficient user privileges.");
        }

        [HttpGet("GetUserBoards")]
        public async Task<IActionResult> GetBoardsByUserAsync([FromQuery] BoardsResourceParameters resourceParameters)
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

            var boards = await _boardService.GetUserBoardsAsync(resourceParameters, userId);

            if (boards is null)
            {
                return NotFound("Cannot find the boards assigned to this user.");
            }

            var metadata = _metadataHelper.CreateMetaData(
                boards, 
                boards.HasPrevious ? 
                    CreateResourceUri(resourceParameters, ResourceUriType.PreviousPage, "GetBoardsAsync") : null,
                boards.HasNext ? 
                    CreateResourceUri(resourceParameters, ResourceUriType.NextPage, "GetBoardsAsync") : null);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var boardsToReturn = _mapper.Map<IEnumerable<BoardForListDto>>(boards);

            return Ok(boardsToReturn);
        }

        [HttpGet(Name = nameof(GetBoardsAsync))]
        public async Task<IActionResult> GetBoardsAsync([FromQuery] BoardsResourceParameters resourceParameters)
        {
            if (!(HttpContext.User.IsInRole(UserRoles.Admin) || HttpContext.User.IsInRole(UserRoles.Mod)))
                return Unauthorized("Insufficient user privileges.");

            var boards = await _boardService.GetBoardsAsync(resourceParameters);

            if (boards == null)
                return NotFound();

            var metadata = _metadataHelper.CreateMetaData(
                boards,

                boards.HasPrevious ? 
                    CreateResourceUri(resourceParameters, ResourceUriType.PreviousPage, "GetBoardsAsync") : null,

                boards.HasNext ? 
                    CreateResourceUri(resourceParameters, ResourceUriType.NextPage, "GetBoardsAsync") : null
            );

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var boardsToReturn = _mapper.Map<IEnumerable<BoardForListDto>>(boards);

            return Ok(boardsToReturn);
        }

        [HttpDelete("{boardId}")]
        public async Task<IActionResult> DeleteBoardAsync(int boardId)
        {
            if (!await _boardService.BoardExistsAsync(boardId))
                return NotFound();

            var boardInDb = await _boardService.GetBoardAsync(boardId);

            if (!IsOwner(boardInDb.UserId) && !await IsEditor(boardId))
                return Unauthorized("Insufficient user privileges.");

            await _boardService.DeleteBoardAsync(boardId);

            if (await _boardService.SaveChangesAsync())
                return Ok();

            return BadRequest("Failed to delete board");

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoardAsync(int id, BoardForEditionDto boardForEditionDto)
        {
            var boardFromRepo = await _boardService.GetBoardAsync(id);

            if (boardFromRepo == null)
                NotFound("Couldn't find the board");

            if (!IsOwner(boardFromRepo.UserId) && !await IsEditor(id))
                return Unauthorized("Insufficient user privileges.");

            _mapper.Map(boardForEditionDto, boardFromRepo);

            if (await _boardService.SaveChangesAsync())
                return NoContent();

            return BadRequest("Failed to update board");
        }

        [HttpPatch("{boardId}")]
        public async Task<IActionResult> PartiallyUpdateBoardAsync(int boardId, JsonPatchDocument<BoardForEditionDto> patchDocument)
        {
            var boardFromRepo = await _boardService.GetBoardAsync(boardId);
            if (boardFromRepo == null)
                return NotFound();

            if (!IsOwner(boardFromRepo.UserId) && !await IsEditor(boardId))
                return Unauthorized("Insufficient user privileges.");

            var boardToPatch = _mapper.Map<BoardForEditionDto>(boardFromRepo);

            patchDocument.ApplyTo(boardToPatch, ModelState);

            if (!TryValidateModel(boardToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(boardToPatch, boardFromRepo);

            if (!await _boardService.SaveChangesAsync())
                return BadRequest();

            return NoContent();
        }

        #region HelperMethods

        [NonAction]
        private bool IsOwner(int? ownerId)
        {
            // Pobranie User Tokenu
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId == ownerId.ToString() ? true : false;
        }

        [NonAction]
        private async Task<bool> IsViewer(int boardId)
        {
            var viewers = await _boardService.GetBoardViewersAsync(boardId);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

            return viewers.ContributorsIds.Contains(userId);
        }

        [NonAction]
        private async Task<bool> IsEditor(int boardId)
        {
            var viewers = await _boardService.GetBoardEditorsAsync(boardId);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

            return viewers.ContributorsIds.Contains(userId);
        }

        [NonAction]
        public override string CreateResourceUri(BoardsResourceParameters resourceParameters, ResourceUriType type, string urlLink)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(urlLink,
                        new
                        {
                            pageNumber = resourceParameters.PageNumber - 1,
                            pageSize = resourceParameters.PageSize,
                            searchQuery = resourceParameters.SearchQuery,
                        });
                case ResourceUriType.NextPage:
                    return Url.Link(urlLink,
                        new
                        {
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize,
                            searchQuery = resourceParameters.SearchQuery,
                        });
                default:
                    return Url.Link(urlLink,
                        new
                        {
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize,
                            searchQuery = resourceParameters.SearchQuery,
                        });
            }
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System.Security.Claims;
using System.Linq;
using Application.Models.Dtos;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;

        public LabelsController(ILabelService labelService, IBoardService boardService, IMapper mapper)
        {
            _mapper = mapper;
            _labelService = labelService;
            _boardService = boardService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLabelAsync(LabelForCreationDto labelForCreationDto)
        {
            var label = _mapper.Map<Label>(labelForCreationDto);

            if (!await _boardService.BoardExistsAsync(label.BoardId))
                return BadRequest($"No Board with ID { label.BoardId } found.");

            if (!await IsOwner(label.BoardId) && !await IsEditor(label.BoardId))
                return Unauthorized("Insufficient user privileges.");

            var labelFromRepo = await _labelService.AddLabelAsync(label);

            if (!await _labelService.SaveChangesAsync())
                return BadRequest();

            var labelToReturn = _mapper.Map<LabelForListDto>(labelFromRepo);

            return CreatedAtRoute("GetLabelAsync",
             new { controller = "Labels", id = labelFromRepo.Id },
              labelToReturn);
        }

        [HttpGet(Name = nameof(GetLabelsAsync))]
        public async Task<IActionResult> GetLabelsAsync()
        {
            if (!(HttpContext.User.IsInRole(UserRoles.Admin) || HttpContext.User.IsInRole(UserRoles.Mod)))
                return Unauthorized("Insufficient user privileges.");

            var labels = await _labelService.GetLabelsAsync();

            if (labels == null)
                return NotFound();

            var labelsToReturn = _mapper.Map<IEnumerable<LabelForListDto>>(labels);

            return Ok(labelsToReturn);
        }

        [HttpGet("board/{boardId}")]
        public async Task<IActionResult> GetLabelsForBoardAsync(int boardId)
        {
            if (!await _boardService.BoardExistsAsync(boardId))
                return BadRequest($"No Board with ID { boardId } found.");

            if (!await IsOwner(boardId) && !await IsViewer(boardId))
                return Unauthorized("Insufficient user privileges.");

            var labels = await _labelService.GetLabelsForBoardAsync(boardId);

            if (labels == null)
                return NotFound();

            return Ok(labels);
        }

        [HttpGet("{id}", Name = nameof(GetLabelAsync))]
        public async Task<IActionResult> GetLabelAsync(int id)
        {
            var label = await _labelService.GetLabelAsync(id);

            if (label == null)
                return BadRequest($"No Label with ID {id} found.");

            if (!await _boardService.BoardExistsAsync(label.BoardId))
                return BadRequest($"No Board with ID { label.BoardId } found.");

            if (!await IsOwner(label.BoardId) && !await IsViewer(label.BoardId))
                return Unauthorized("Insufficient user privileges.");

            var labelToReturn = _mapper.Map<LabelForListDto>(label);

            return Ok(labelToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabelAsync(int id)
        {
            if (!await _labelService.LabelExistsAsync(id))
                return NotFound();

            var label = await _labelService.GetLabelAsync(id);

            if (!await IsOwner(label.BoardId) && !await IsEditor(label.BoardId))
                return Unauthorized("Insufficient user privileges.");

            await _labelService.DeleteLabelAsync(id);

            if (await _labelService.SaveChangesAsync())
                return Ok();

            return BadRequest("Failed to delete label");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabelAsync(int id, LabelForEditionDto labelForEditionDto)
        {
            var labelFromRepository = await _labelService.GetLabelAsync(id);

            if (labelFromRepository == null)
                return NotFound();

            if (!await IsOwner(labelFromRepository.BoardId) && !await IsEditor(labelFromRepository.BoardId))
                return Unauthorized("Insufficient user privileges.");

            _mapper.Map(labelForEditionDto, labelFromRepository);

            if (await _labelService.SaveChangesAsync())
                return NoContent();

            return BadRequest("Failed to update label");
        }

        [HttpPatch("{labelId}")]
        public async Task<IActionResult> PartiallyUpdateLabelAsync(int labelId, JsonPatchDocument<LabelForEditionDto> patchDocument)
        {
            var labelFromRepo = await _labelService.GetLabelAsync(labelId);

            if (labelFromRepo == null)
                return NotFound();

            if (!await IsOwner(labelFromRepo.BoardId) && !await IsEditor(labelFromRepo.BoardId))
                return Unauthorized("Insufficient user privileges.");

            var labelToPatch = _mapper.Map<LabelForEditionDto>(labelFromRepo);

            patchDocument.ApplyTo(labelToPatch, ModelState);

            if (!TryValidateModel(labelToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(labelToPatch, labelFromRepo);

            if (!await _labelService.SaveChangesAsync())
                return BadRequest();

            return NoContent();
        }

        #region HelperMethods
        [NonAction]
        public async Task<bool> IsOwner(int boardId)
        {
            var boardInDb = await _boardService.GetBoardAsync(boardId);

            return boardInDb.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [NonAction]
        private async Task<bool> IsEditor(int boardId)
        {
            var viewers = await _boardService.GetBoardEditorsAsync(boardId);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

            return viewers.ContributorsIds.Contains(userId);
        }
        [NonAction]
        public async Task<bool> IsViewer(int boardId)
        {
            var viewers = await _boardService.GetBoardViewersAsync(boardId);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

            return viewers.ContributorsIds.Contains(userId);
        }
        #endregion
    }
}
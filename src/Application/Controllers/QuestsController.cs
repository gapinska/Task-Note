using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Application.Interfaces;
using Domain.ResourceParameters;
using Domain.Entities;
using Domain;
using System.Security.Claims;
using Infrastructure.Interfaces;
using Application.Controllers.ControllerModels;
using System.Security.AccessControl;
using System.Linq;
using Application.Models.Dtos;

namespace Application.Controllers
{
    [Route("api/boards/{boardId}/[controller]")]
    [ApiController]
    public class QuestsController : ControllerBaseWithUri<QuestsResourceParameters>
    {
        private readonly IMetadataHelper<Quest> _metadataHelper;
        private readonly IQuestService _questService;
        private readonly IBoardService _boardService;
        private readonly IMapper _mapper;

        public QuestsController(IQuestService questService, IBoardService boardService, IMapper mapper, IMetadataHelper<Quest> metadataHelper)
        {
            _metadataHelper = metadataHelper;
            _questService = questService;
            _boardService = boardService;
            _mapper = mapper;
        }

        [HttpGet("{questId}", Name = nameof(GetQuestAsync))]
        public async Task<IActionResult> GetQuestAsync(int questId, int boardId)
        {
            if (!await _boardService.BoardExistsAsync(boardId))
                return BadRequest($"No Board with ID { boardId } found.");

            if (!await IsOwner(boardId) && !await IsViewer(boardId))
                return Unauthorized("Insufficient user privileges.");

            var quest = await _questService.GetQuestAsync(questId);

            if (quest == null) return NotFound();

            if (quest.BoardId != boardId) BadRequest();

            var questToReturn = _mapper.Map<QuestForListDto>(quest);

            return Ok(questToReturn);
        }

        [HttpGet(Name = nameof(GetQuestsAsync))]
        public async Task<IActionResult> GetQuestsAsync(int boardId, [FromQuery] QuestsResourceParameters resourceParameters)
        {
            if (!await _boardService.BoardExistsAsync(boardId))
                return BadRequest($"No Board with ID { boardId } found.");

            if (!await IsOwner(boardId) && !await IsViewer(boardId))
                return Unauthorized("Insufficient user privileges.");

            var quests = await _questService.GetQuestsAsync(boardId, resourceParameters);

            if (quests == null)
                return NotFound();

            var metadata = _metadataHelper.CreateMetaData(
                quests,

                quests.HasPrevious ? 
                    CreateResourceUri(resourceParameters, ResourceUriType.PreviousPage, "GetQuestsAsync") : null,

                quests.HasNext ? 
                    CreateResourceUri(resourceParameters, ResourceUriType.NextPage, "GetQuestsAsync") : null
            );

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var questsToReturn = _mapper.Map<IEnumerable<QuestForListDto>>(quests);

            return Ok(questsToReturn);
        }

        [HttpDelete("{questId}")]
        public async Task<IActionResult> DeleteQuestAsync(int questId)
        {
            if (!await _questService.QuestExistsAsync(questId))
                return NotFound();

            var questInDb = await _questService.GetQuestAsync(questId);

            if (!await IsOwner((int)questInDb.BoardId) && !await IsEditor((int)questInDb.BoardId))
                return Unauthorized("Insufficient user privileges.");

            await _questService.DeleteQuestAsync(questId);

            if (await _questService.SaveChangesAsync())
                return Ok();

            return BadRequest("Failed to delete quest");
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestAsync(int boardId, QuestForCreationDto questForCreationDto)
        {
            if (!await _boardService.BoardExistsAsync(boardId))
                return BadRequest($"No Board with ID { boardId } found.");

            if (!await IsOwner(boardId) && !await IsEditor(boardId))
                return Unauthorized("Insufficient user privileges.");

            questForCreationDto.BoardId = boardId;
            var quest = _mapper.Map<Quest>(questForCreationDto);
            var questFromRepo = await _questService.AddQuestAsync(quest);

            if (!await _questService.SaveChangesAsync())
                return BadRequest();

            var questToReturn = _mapper.Map<QuestForListDto>(questFromRepo);

            return CreatedAtRoute("GetQuestAsync",
             new { controller = "Quests", questId = questFromRepo.Id, boardId = questFromRepo.BoardId },
              questToReturn);
        }

        [HttpPut("{questId}")]
        public async Task<IActionResult> UpdateQuestAsync(int questId, int boardId, QuestForEditionDto questForEditionDto)
        {
            if (!await _boardService.BoardExistsAsync(boardId))
                return BadRequest($"No Board with ID { boardId } found.");

            if (!await IsOwner(boardId) && !await IsEditor(boardId))
            {
                return Unauthorized("Insufficient user privileges.");
            }

            var questFromRepository = await _questService.GetQuestAsync(questId);

            if (questFromRepository == null)
                return NotFound();

            if (questFromRepository.BoardId != boardId)
                return BadRequest();

            _mapper.Map(questForEditionDto, questFromRepository);
            if (await _questService.SaveChangesAsync())
                return NoContent();

            return BadRequest("Failed to update quest");
        }

        [HttpPatch("{questId}")]
        public async Task<IActionResult> PartiallyUpdateQuestAsync(int questId, int boardId, JsonPatchDocument<QuestForEditionDto> patchDocument)
        {
            if (!await _boardService.BoardExistsAsync(boardId))
                return BadRequest($"No Board with ID { boardId } found.");

            if (!await IsOwner(boardId) && !await IsEditor(boardId))
            {
                return Unauthorized("Insufficient user privileges.");
            }

            var questFromRepo = await _questService.GetQuestAsync(questId);

            if (questFromRepo == null)
                return NotFound();

            if (questFromRepo.BoardId != boardId)
                return BadRequest();

            var questToPatch = _mapper.Map<QuestForEditionDto>(questFromRepo);

            patchDocument.ApplyTo(questToPatch, ModelState);

            if (!TryValidateModel(questToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(questToPatch, questFromRepo);

            if (!await _questService.SaveChangesAsync())
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
        public async Task<bool> IsViewer(int boardId)
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
        public override string CreateResourceUri(QuestsResourceParameters resourceParameters, ResourceUriType type, string urlLink)
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
                            labelId = resourceParameters.LabelId,
                            boardId = resourceParameters.BoardId,
                            isDone = resourceParameters.IsDone,
                            daysToDeadline = resourceParameters.DaysToDeadline
                        });
                case ResourceUriType.NextPage:
                    return Url.Link(urlLink,
                        new
                        {
                            pageNumber = resourceParameters.PageNumber + 1,
                            pageSize = resourceParameters.PageSize,
                            searchQuery = resourceParameters.SearchQuery,
                            labelId = resourceParameters.LabelId,
                            boardId = resourceParameters.BoardId,
                            isDone = resourceParameters.IsDone,
                            daysToDeadline = resourceParameters.DaysToDeadline
                        });
                default:
                    return Url.Link(urlLink,
                        new
                        {
                            pageNumber = resourceParameters.PageNumber,
                            pageSize = resourceParameters.PageSize,
                            searchQuery = resourceParameters.SearchQuery,
                            labelId = resourceParameters.LabelId,
                            boardId = resourceParameters.BoardId,
                            isDone = resourceParameters.IsDone,
                            daysToDeadline = resourceParameters.DaysToDeadline
                        });
            }
        }

        #endregion
    }
}
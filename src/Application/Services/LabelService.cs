using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IQuestRepository _questRepository;
        public LabelService(ILabelRepository labelRepository, IBoardRepository boardRepository,
        IQuestRepository questRepository)
        {
            _questRepository = questRepository;
            _boardRepository = boardRepository;
            _labelRepository = labelRepository;
        }

        public async Task<Label> AddLabelAsync(Label label)
        {
            await _labelRepository.AddLabelAsync(label);

            return label;
        }

        public async Task DeleteLabelAsync(int id)
        {
            await _labelRepository.DeleteLabelAsync(id);
        }

        public async Task<Label> GetLabelAsync(int id)
        {
            var label = await _labelRepository.GetLabelAsync(id);

            return label;
        }

        public async Task<IEnumerable<Label>> GetLabelsAsync()
        {
            var labels = await _labelRepository.GetLabelsAsync();

            return labels;
        }

        public async Task<IEnumerable<Label>> GetLabelsForBoardAsync(int boardId)
        {
            var board = await _boardRepository.GetBoardAsync(boardId);

            var labelsInThisBoard = board.Labels;

            return labelsInThisBoard;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _labelRepository.SaveChangesAsync();
        }

        public async Task<bool> LabelExistsAsync(int id)
        {
            return await _labelRepository.LabelExistsAsync(id);
        }
    }
}
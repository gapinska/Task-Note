using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.ResourceParameters;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class QuestService : IQuestService
    {
        private readonly IQuestRepository _questRepository;
        public QuestService(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        public async Task<Quest> AddQuestAsync(Quest quest)
        {
            await _questRepository.AddQuestAsync(quest);

            return quest;
        }

        public async Task DeleteQuestAsync(int id)
        {
            await _questRepository.DeleteQuestAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _questRepository.SaveAllAsync();
        }

        public async Task<Quest> GetQuestAsync(int id)
        {
            return await _questRepository.GetQuestAsync(id);
        }

        public async Task<PagedList<Quest>> GetQuestsAsync(int boardId, QuestsResourceParameters resourceParameters)
        {
            return await _questRepository.GetQuestsAsync(boardId, resourceParameters);
        }

        public async Task MarkAsDoneAsync(int id)
        {
            await _questRepository.MarkAsDoneAsync(id);
        }

        public async Task MarkAsUndoneAsync(int id)
        {
            await _questRepository.MarkAsUndoneAsync(id);
        }

        public async Task SetDeadlineAsync(int id, DateTime date)
        {
            await _questRepository.SetDeadlineAsync(id, date);
        }

        public async Task DeleteDeadlineAsync(int id)
        {
            await _questRepository.DeleteDeadlineAsync(id);
        }

        public async Task<bool> QuestExistsAsync(int id)
        {
            return await _questRepository.QuestExistsAsync(id);
        }
    }
}
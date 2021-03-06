using Domain;
using Domain.Entities;
using Domain.ResourceParameters;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IQuestRepository
    {
        Task<Quest> GetQuestAsync(int id);
        Task<PagedList<Quest>> GetQuestsAsync(int boardId, QuestsResourceParameters resourceParameters);
        Task<Quest> GetQuestByLabelIdAsync(int id);
        Task DeleteQuestAsync(int id);
        Task<Quest> AddQuestAsync(Quest quest);
        Task<bool> SaveAllAsync();
        Task MarkAsDoneAsync(int id);
        Task MarkAsUndoneAsync(int id);
        Task SetDeadlineAsync(int id, DateTime date);
        Task DeleteDeadlineAsync(int id);
        Task<bool> QuestExistsAsync(int id);
    }
}
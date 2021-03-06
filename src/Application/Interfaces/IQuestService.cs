using Domain;
using Domain.Entities;
using Domain.ResourceParameters;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQuestService
    {
        Task<Quest> GetQuestAsync(int id);
        Task<PagedList<Quest>> GetQuestsAsync(int boardId, QuestsResourceParameters resourceParameters);
        Task DeleteQuestAsync(int id);
        Task<Quest> AddQuestAsync(Quest quest);
        Task<bool> SaveChangesAsync();
        Task MarkAsDoneAsync(int id);
        Task MarkAsUndoneAsync(int id);
        Task SetDeadlineAsync(int id, DateTime date);
        Task DeleteDeadlineAsync(int id);
        Task<bool> QuestExistsAsync(int id);
    }
}
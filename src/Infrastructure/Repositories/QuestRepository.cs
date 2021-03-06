using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ResourceParameters;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QuestRepository : IQuestRepository
    {
        private readonly DataContext _context;
        private readonly ISortHelper<Quest> _sortHelper;

        public QuestRepository(DataContext context, ISortHelper<Quest> sortHelper)
        {
            _context = context;
            _sortHelper = sortHelper;
        }

        public async Task<Quest> GetQuestAsync(int id)
        {
            return await _context.Quests.SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<PagedList<Quest>> GetQuestsAsync(int boardId, QuestsResourceParameters resourceParameters)
        {
            var collection = _context.Quests as IQueryable<Quest>;

            collection = collection.Where(q => q.BoardId == boardId);

            if (resourceParameters.LabelId != null)
            {
                collection = collection.Where(q => q.LabelId == resourceParameters.LabelId);
            }

            if (resourceParameters.IsDone != null)
            {
                collection = collection.Where(q => q.IsDone == resourceParameters.IsDone);
            }

            if (resourceParameters.DaysToDeadline != null)
            {
                collection = collection.Where(q => (q.Deadline != null)
                                                   &&
                                                   (q.Deadline > DateTime.Today
                                                   || (q.Deadline - DateTime.Today).Value.Days <=
                                                   resourceParameters.DaysToDeadline));
            }

            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                var searchQuery = resourceParameters.SearchQuery.Trim();
                collection = collection.Where(q => q.Name.Contains(searchQuery,
                    StringComparison.OrdinalIgnoreCase)
                || q.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            var sortedQuests = _sortHelper.ApplySort(collection, resourceParameters.OrderBy);

            var collectionToReturn = await PagedList<Quest>.ToPagedListAsync(sortedQuests,
                resourceParameters.PageNumber, resourceParameters.PageSize);

            return collectionToReturn;
        }

        public async Task DeleteQuestAsync(int id)
        {
            var questToRemove = await GetQuestAsync(id);
            _context.Remove(questToRemove);
        }

        public async Task<Quest> AddQuestAsync(Quest quest)
        {
            quest.IsDone = false;
            await _context.Quests.AddAsync(quest);
            return quest;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task MarkAsDoneAsync(int id)
        {
            var questInDb = await GetQuestAsync(id);
            questInDb.IsDone = true;
            questInDb.CompletionDate = DateTime.Today;
        }

        public async Task MarkAsUndoneAsync(int id)
        {
            var questInDb = await GetQuestAsync(id);
            questInDb.IsDone = false;
            questInDb.CompletionDate = null;
        }

        public async Task SetDeadlineAsync(int id, DateTime date)
        {
            var quest = await GetQuestAsync(id);
            quest.Deadline = date;
        }

        public async Task DeleteDeadlineAsync(int id)
        {
            var quest = await GetQuestAsync(id);
            quest.Deadline = null;
        }

        public Task<Quest> GetQuestByLabelIdAsync(int id)
        {
            var quest = _context.Quests.SingleOrDefaultAsync(q => q.LabelId == id);

            return quest;
        }

        public async Task<bool> QuestExistsAsync(int id)
        {
            return await _context.Quests.AnyAsync(q => q.Id == id);
        }
    }
}
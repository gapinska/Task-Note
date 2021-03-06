using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILabelService
    {
        Task<Label> AddLabelAsync(Label label);
        Task<IEnumerable<Label>> GetLabelsAsync();
        Task<IEnumerable<Label>> GetLabelsForBoardAsync(int boardId);
        Task<Label> GetLabelAsync(int id);
        Task DeleteLabelAsync(int id);
        Task<bool> SaveChangesAsync();
        Task<bool> LabelExistsAsync(int id);
    }
}
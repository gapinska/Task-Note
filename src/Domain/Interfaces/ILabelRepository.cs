using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILabelRepository
    {
        Task<Label> AddLabelAsync(Label label);
        Task<IEnumerable<Label>> GetLabelsAsync();
        Task<Label> GetLabelAsync(int id);
        Task DeleteLabelAsync(int id);
        Task<bool> SaveChangesAsync();
        Task<bool> LabelExistsAsync(int id);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly DataContext _context;
        public LabelRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Label> AddLabelAsync(Label label)
        {
            await _context.Labels.AddAsync(label);
            return label;
        }

        public async Task DeleteLabelAsync(int id)
        {
            var labelToRemove = await GetLabelAsync(id);
            _context.Labels.Remove(labelToRemove);
        }

        public async Task<IEnumerable<Label>> GetLabelsAsync()
        {
            var labels = await _context.Labels.ToListAsync();
            return labels;
        }

        public async Task<Label> GetLabelAsync(int id)
        {
            var label = await _context.Labels.SingleOrDefaultAsync(l => l.Id == id);
            return label;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LabelExistsAsync(int id)
        {
            return await _context.Labels.AnyAsync(l => l.Id == id);
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Domain.Entities;
using Domain;
using Domain.ResourceParameters;
using Infrastructure.Interfaces;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly ISortHelper<User> _sortHelper;

        public UserRepository(DataContext context, ISortHelper<User> sortHelper)
        {
            _context = context;
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<User>> GetUsersAsync(UsersResourceParameters resourceParameters)
        {
            var collection = _context.Users as IQueryable<User>;

            if (!string.IsNullOrWhiteSpace(resourceParameters.Role))
            {
                var roleName = resourceParameters.Role.Trim();
                collection = collection.Where(u => u.UserRoles.Any(r => r.Role.Name == roleName));
            }

            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                var searchQuery = resourceParameters.SearchQuery.Trim();
                collection = collection.Where(u => u.FullName.Contains(searchQuery,
                                                       StringComparison.OrdinalIgnoreCase)
                                                   || u.Email.Contains(searchQuery,
                                                       StringComparison.OrdinalIgnoreCase)
                                                   || u.UserName.Contains(searchQuery,
                                                       StringComparison.OrdinalIgnoreCase));
            }

            var sortedUsers = _sortHelper.ApplySort(collection, resourceParameters.OrderBy);

            var collectionToReturn = await PagedList<User>
                .ToPagedListAsync(sortedUsers, resourceParameters.PageNumber, resourceParameters.PageSize);

            return collectionToReturn;
        }

        public async Task DeleteUserAsync(int id)
        {
            var userToRemove = await GetUserByIdAsync(id);
            _context.Users.Remove(userToRemove);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                string.Equals(u.UserName, name, StringComparison.CurrentCultureIgnoreCase));

            return user;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }
    }
}
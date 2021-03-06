using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Domain.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;

        public AdminRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var userList = await _context.Users.ToListAsync();
            return userList;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }
    }
}
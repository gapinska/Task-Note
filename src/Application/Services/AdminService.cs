using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models.Dtos;
using Domain.Interfaces;
using System.Linq;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<IEnumerable<UserWithRolesDto>> GetUsersWithRolesAsync()
        {
            var users = await _adminRepository.GetUsers();
            var roles = await _adminRepository.GetRoles();

            var usersWithRoles = users.OrderBy(u => u.UserName)
                .Select(user => new UserWithRolesDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = (from userRole in user.UserRoles
                             join role in roles
                                 on userRole.RoleId
                                 equals role.Id
                             select role.Name).ToList()
                });

            return usersWithRoles;
        }
    }
}
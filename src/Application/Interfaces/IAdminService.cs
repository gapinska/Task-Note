using Application.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserWithRolesDto>> GetUsersWithRolesAsync();
    }
}
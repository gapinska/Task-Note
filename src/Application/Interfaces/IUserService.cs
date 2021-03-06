using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.ResourceParameters;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string name);
        Task<PagedList<User>> GetUsersAsync(UsersResourceParameters resourceParameters);
        Task DeleteUserAsync(int id);
        Task<bool> SaveChangesAsync();
        Task<bool> UserExistsAsync(int id);
    }
}
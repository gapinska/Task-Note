using Domain;
using Domain.Entities;
using Domain.ResourceParameters;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string name);
        Task<PagedList<User>> GetUsersAsync(UsersResourceParameters resourceParameters);
        Task DeleteUserAsync(int id);
        Task<bool> SaveChangesAsync();
        Task<bool> UserExistsAsync(int id);
    }
}
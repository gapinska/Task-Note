using System.Threading.Tasks;
using Application.Interfaces;
using Domain.ResourceParameters;
using Domain.Entities;
using Domain;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            return user;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var user = await _userRepository.GetUserByNameAsync(name);

            return user;
        }

        public async Task<PagedList<User>> GetUsersAsync(UsersResourceParameters resourceParameters)
        {
            return await _userRepository.GetUsersAsync(resourceParameters);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _userRepository.SaveChangesAsync();
        }
        public async Task<bool> UserExistsAsync(int id)
        {
            return await _userRepository.UserExistsAsync(id);
        }
    }
}
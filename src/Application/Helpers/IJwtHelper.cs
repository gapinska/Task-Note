using System.Threading.Tasks;
using Application.Models.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Helpers
{
    public interface IJwtHelper
    {
        public Task<AuthenticationResultDto> GenerateJwtTokenAsync(User user, UserManager<User> userManager, IConfiguration configuration);

        public Task<AuthenticationResultDto> RefreshTokenAsync(string accessToken, string refreshToken,
            UserManager<User> userManager, IConfiguration configuration);
    }
}
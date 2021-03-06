using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Dtos;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Helpers
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public JwtHelper(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public async Task<AuthenticationResultDto> GenerateJwtTokenAsync(User user, UserManager<User> userManager, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(2)
            };

            await _dataContext.RefreshTokens.AddAsync(refreshToken);

            await _dataContext.SaveChangesAsync();

            return new AuthenticationResultDto
            {
                Success = true,
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthenticationResultDto> RefreshTokenAsync(string accessToken, string refreshToken, UserManager<User> userManager, IConfiguration configuration)
        {
            var validatedToken = GetPrincipalFromToken(accessToken);
            
            var errorResult =  new AuthenticationResultDto
            {
                Success = false,
            };

            if (validatedToken == null)
                return errorResult;

            if (CheckIfExpired(validatedToken))
                return errorResult;
            
            // JWT id
            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (ValidateRefreshToken(storedRefreshToken, jti))
                return errorResult;
            
            storedRefreshToken.Used = true;
            
            _dataContext.RefreshTokens.Update(storedRefreshToken);

            await _dataContext.SaveChangesAsync();

            var user = await userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier)
                .Value);

            return await GenerateJwtTokenAsync(user, userManager, configuration);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding
                    .ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        private bool CheckIfExpired(ClaimsPrincipal validatedToken)
        {
            var expiryDateTimeUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateTimeUnix);

            return (expiryDateTimeUtc <= DateTime.UtcNow);
        }

        private bool ValidateRefreshToken(RefreshToken storedRefreshToken, string jti)
        {
            return (
                storedRefreshToken == null
                ||
                DateTime.UtcNow > storedRefreshToken.ExpiryDate
                ||
                storedRefreshToken.Invalidated || storedRefreshToken.Used
                ||
                storedRefreshToken.JwtId != jti
            );
        }
    }
}
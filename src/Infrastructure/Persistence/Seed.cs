using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Persistence
{
    public static class Seed
    {
        public static async Task SeedUsersAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("../Infrastructure/Persistence/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            //create roles
            var roles = new List<Role>
            {
                new Role {Name = "Member"},
                new Role {Name = "Administrator"}, 
                new Role {Name = "Moderator"}
            };

            foreach (var role in roles)
            { 
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            { 
                await userManager.CreateAsync(user, "P@ssword123"); 
                await userManager.AddToRoleAsync(user, "Member");
            }

            //create admin
            var adminUser = new User
            {
                UserName = "Admin"
            };

            var adminResult = userManager.CreateAsync(adminUser, "P@ssword123").Result;

            if (!adminResult.Succeeded) return;
            
            var admin = userManager.FindByNameAsync("Admin").Result;
            await userManager.AddToRolesAsync(admin, new[] {"Administrator", "Moderator"});
            
            //create moderator
            var moderatorUser = new User()
            {
                UserName = "Moderator"
            };
            var moderatorResult = userManager.CreateAsync(moderatorUser, "P@ssword123").Result;

            if (!moderatorResult.Succeeded) return;
            var moderator = userManager.FindByNameAsync("Moderator").Result;
            await userManager.AddToRoleAsync(moderator, "Moderator");
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
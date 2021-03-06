using System;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Application.Helpers
{
    public static class ConfigurationServicesCollectionExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding
                        .ASCII.GetBytes(config.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chat")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAdminRole", policy => { policy.RequireRole("Administrator"); });
                    options.AddPolicy("RequireModeratorOrAdminRole", policy => { policy.RequireRole("Administrator", "Moderator"); });
                });
        }

        public static void ConfigureServicesAndRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();
            services.AddScoped<ILabelService, LabelService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<ISortHelper<User>, SortHelper<User>>();
            services.AddScoped<ISortHelper<Quest>, SortHelper<Quest>>();
            services.AddScoped<ISortHelper<Board>, SortHelper<Board>>();
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IMetadataHelper<Quest>, MetadataHelper<Quest>>();
            services.AddScoped<IMetadataHelper<Board>, MetadataHelper<Board>>();
            
            services.AddSingleton<IConfiguration>(configuration);
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TaskNote API",
                    Description = "TaskNote API - team's quests management app",
                    Contact = new OpenApiContact
                    {
                        Name = "Łukasz Czyż",
                        Email = "czyzukasz2402@gmail.com",
                        Url = new Uri("https://kodev.pl")
                    }
                });
            });
        }
    }
}
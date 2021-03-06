using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Application.Helpers
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskNote API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
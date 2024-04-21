using Microsoft.Extensions.Configuration;
using OtpServer.Controllers.Policy;

namespace OtpServer.Config
{
    public static class CorsConfigurationServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicies.AllowOrigins, builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }
    }
}

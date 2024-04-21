using OtpServer.Controllers.Policy;
using OtpServer.Security.Jwt;
using OtpServer.Security.Requirement;

namespace OtpServer.Config
{
    public static class AuthorizationServiceExtension
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.MainLoginPolicy, policy =>
                    policy.AddRequirements(new TokenTypeRequirement(TokenType.MainLogin)));

                options.AddPolicy(Policies.OtpLoginPolicy, policy =>
                    policy.AddRequirements(new TokenTypeRequirement(TokenType.OtpLogin)));
            });
        }
    }
}

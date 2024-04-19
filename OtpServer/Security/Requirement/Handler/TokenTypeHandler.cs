using Microsoft.AspNetCore.Authorization;
using OtpServer.Security.Jwt;

namespace OtpServer.Security.Requirement.Handler
{
    public class TokenTypeHandler : AuthorizationHandler<TokenTypeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenTypeRequirement requirement)
        {
            var user = context.User;
            var tokenTypeClaim = user.FindFirst(JwtClaims.TokenType);

            if (tokenTypeClaim == null || tokenTypeClaim.Value != requirement.RequiredTokenType.ToString()) 
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using OtpServer.Security.Jwt;

namespace OtpServer.Security.Requirement
{
    public class TokenTypeRequirement : IAuthorizationRequirement
    {
        public TokenType RequiredTokenType { get; }

        public TokenTypeRequirement(TokenType requiredTokenType)
        {
            RequiredTokenType = requiredTokenType;
        }   
    }
}

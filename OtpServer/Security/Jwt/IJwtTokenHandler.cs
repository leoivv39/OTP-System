using OtpServer.Repository.Model;
using System.Security.Claims;

namespace OtpServer.Security.Jwt
{
    public interface IJwtTokenHandler
    {
        string GenerateJwtToken(User user, TokenType tokenType);

        string FindClaim(string claimName, ClaimsPrincipal user);
    }
}

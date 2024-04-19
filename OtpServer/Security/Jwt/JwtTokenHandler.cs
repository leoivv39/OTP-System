using Microsoft.IdentityModel.Tokens;
using OtpServer.Exception;
using OtpServer.Repository.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OtpServer.Security.Jwt
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly IConfiguration _config;

        public JwtTokenHandler(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(User user, TokenType tokenType)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtClaims.TokenType, tokenType.ToString()),
                new Claim(JwtClaims.UserUid, user.Uid.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_config["Jwt:TokenExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string FindClaim(string claimName, ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type == claimName);
            if (claim == null)
            {
                throw new UnauthorizedException();
            }
            return claim.Value;
        }
    }
}

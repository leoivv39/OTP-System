using OtpServer.Dto;
using OtpServer.Encryption;
using OtpServer.Mapper;
using OtpServer.Repository.Model;
using OtpServer.Request;
using OtpServer.Security.Jwt;
using OtpServer.Service;

namespace OtpServer.Facade
{
    public class UserFacade(IUserService userService, IUserMapper userMapper, IJwtTokenHandler jwtTokenHandler, IEncryptionHandler encryptionHandler)
        : IUserFacade
    {
        public async Task<UserDto> AddUserAsync(CreateUserRequest request)
        {
            var decryptedPassword = encryptionHandler.Decrypt(request.Password);
            request.Password = decryptedPassword;
            var userToAdd = userMapper.ToUser(request);
            var addedUser = await userService.AddUserAsync(userToAdd);
            return userMapper.ToDto(addedUser);
        }

        public async Task<string> LoginUserAsync(LoginRequest request)
        {
            var decryptedPassword = encryptionHandler.Decrypt(request.Password);
            request.Password = decryptedPassword;
            User user = await userService.GetUserByUsernameAndPasswordAsync(request.Username, request.Password);
            return jwtTokenHandler.GenerateJwtToken(user, TokenType.MainLogin);
        }
    }
}

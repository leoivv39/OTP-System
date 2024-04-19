using OtpServer.Dto;
using OtpServer.Request;

namespace OtpServer.Facade
{
    public interface IUserFacade
    {
        Task<UserDto> AddUserAsync(CreateUserRequest request);

        Task<string> LoginUserAsync(LoginRequest request);
    }
}

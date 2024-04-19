using Microsoft.AspNetCore.Mvc;
using OtpServer.Dto;
using OtpServer.Request;

namespace OtpServer.Controllers
{
    public interface IAuthController
    {
        Task<ActionResult<UserDto>> RegisterUser(CreateUserRequest request);

        Task<ActionResult<string>> LoginUser(LoginRequest request);

        Task<ActionResult<OtpItemDto>> GenerateOtp();

        Task<ActionResult<string>> LoginUserWithOtp(LoginWithOtpRequest request);

        ActionResult<string> LoggedInWithOtpOnly();
    }
}

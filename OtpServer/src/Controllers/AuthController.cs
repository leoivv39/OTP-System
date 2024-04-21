using Microsoft.AspNetCore.Mvc;
using OtpServer.Dto;
using OtpServer.Facade;
using OtpServer.Request;
using Microsoft.AspNetCore.Authorization;
using OtpServer.Controllers.Policy;

namespace OtpServer.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController(IUserFacade userFacade, IOtpFacade otpFacade) : ControllerBase, IAuthController
    {
        [HttpPost]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedUser = await userFacade.AddUserAsync(request);
            return Created(string.Empty, addedUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginRequest request)
        {
            string token = await userFacade.LoginUserAsync(request);
            return Ok(token);
        }

        [HttpPost("otp")]
        [Authorize(Policy = Policies.MainLoginPolicy)]
        public async Task<ActionResult<OtpItemDto>> GenerateOtp()
        {
            var otpItemDto = await otpFacade.GenerateOtpAsync();
            return Created(string.Empty, otpItemDto);
        }

        [HttpPost("otp/login")]
        [Authorize(Policy = Policies.MainLoginPolicy)]
        public async Task<ActionResult<string>> LoginUserWithOtp([FromBody] LoginWithOtpRequest request)
        {
            string token = await otpFacade.LoginUserWithOtpAsync(request);
            return Ok(token);
        }

        [HttpGet("otp/only")]
        [Authorize(Policy = Policies.OtpLoginPolicy)]
        public ActionResult<string> LoggedInWithOtpOnly()
        {
            return Ok("Only users logged in with an OTP can access this.");
        }
    }
}

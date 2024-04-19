using Microsoft.AspNetCore.Mvc;
using OtpServer.Dto;
using OtpServer.Facade;
using OtpServer.Request;
using Microsoft.AspNetCore.Authorization;
using OtpServer.Controllers.Policy;

namespace OtpServer.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IUserFacade _userFacade;
        private readonly IOtpFacade _otpFacade;

        public AuthController(IUserFacade userFacade, IOtpFacade otpFacade)
        {
            _userFacade = userFacade;
            _otpFacade = otpFacade;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addedUser = await _userFacade.AddUserAsync(request);
            return CreatedAtAction(nameof(RegisterUser), addedUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginRequest request)
        {
            string token = await _userFacade.LoginUserAsync(request);
            return Ok(token);
        }

        [HttpPost("otp")]
        [Authorize(Policy = Policies.MainLoginPolicy)]
        public async Task<ActionResult<OtpItemDto>> GenerateOtp()
        {
            var otpItemDto = await _otpFacade.GenerateOtpAsync();
            return CreatedAtAction(nameof(GenerateOtp), otpItemDto);
        }

        [HttpPost("otp/login")]
        [Authorize(Policy = Policies.MainLoginPolicy)]
        public async Task<ActionResult<string>> LoginUserWithOtp(LoginWithOtpRequest request)
        {
            string token = await _otpFacade.LoginUserWithOtpAsync(request);
            return Ok(token);
        }

        [HttpPost("otp/only")]
        [Authorize(Policy = Policies.OtpLoginPolicy)]
        public ActionResult<string> LoggedInWithOtpOnly()
        {
            return Ok("Only users logged in with an OTP can access this.");
        }
    }
}

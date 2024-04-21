using Microsoft.AspNetCore.Mvc;
using Moq;
using OtpServer.Controllers;
using OtpServer.Dto;
using OtpServer.Exception;
using OtpServer.Facade;
using OtpServer.Request;

namespace OtpServer.Tests.Unit.Controller
{
    public class AuthControllerTest
    {
        private readonly IAuthController _authController;
        private readonly Mock<IUserFacade> _userFacadeMock;
        private readonly Mock<IOtpFacade> _otpFacadeMock;

        public AuthControllerTest()
        {
            _userFacadeMock = new Mock<IUserFacade>();
            _otpFacadeMock = new Mock<IOtpFacade>();
            _authController = new AuthController(_userFacadeMock.Object, _otpFacadeMock.Object);
        }

        [Fact]
        public async Task RegisterUser_ReturnsCreatedActionResult()
        {
            var createUserRequest = new CreateUserRequest();
            var createdUser = new UserDto();
            _userFacadeMock.Setup(userFacade => userFacade.AddUserAsync(createUserRequest))
                .ReturnsAsync(createdUser);

            var actionResult = await _authController.RegisterUser(createUserRequest);

            _userFacadeMock.Verify(facade => facade.AddUserAsync(createUserRequest), Times.Once);
            Assert.NotNull(actionResult);
            var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
            Assert.Equal(createdUser, createdResult.Value);
        }

        [Fact]
        public async Task LoginUser_UserDoesNotExist_ThrowsUserNotFoundException()
        {
            var loginRequest = new LoginRequest();
            _userFacadeMock.Setup(userFacade => userFacade.LoginUserAsync(loginRequest))
                .ThrowsAsync(new UserNotFoundException("message"));

            await Assert.ThrowsAsync<UserNotFoundException>(() => _authController.LoginUser(loginRequest));
        }

        [Fact]
        public async Task LoginUser_UserExists_ReturnsOkActionResult()
        {
            var loginRequest = new LoginRequest();
            var jwtToken = "token";
            _userFacadeMock.Setup(userFacade => userFacade.LoginUserAsync(loginRequest))
                .ReturnsAsync(jwtToken);

            var actionResult = await _authController.LoginUser(loginRequest);

            Assert.NotNull(actionResult);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(jwtToken, okResult.Value);
        }

        [Fact]
        public async Task GenerateOtp_ReturnsCreatedActionResult()
        {
            var createdItem = new OtpItemDto();
            _otpFacadeMock.Setup(otpFacade => otpFacade.GenerateOtpAsync())
                .ReturnsAsync(createdItem);

            var actionResult = await _authController.GenerateOtp();

            Assert.NotNull(actionResult);
            var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
            Assert.Equal(createdItem, createdResult.Value);
        }

        [Theory]
        [InlineData(typeof(InvalidOtpException), "Invalid OTP message")]
        [InlineData(typeof(ExpiredOtpException), "Expired OTP message")]
        [InlineData(typeof(ConsumedOtpException), "Consumed OTP message")]
        public async Task LoginWithOtp_BadLoginRequest_ThrowsExpectedException(Type exceptionType, string message)
        {
            var loginRequest = new LoginWithOtpRequest();
            var expectedException = (ApiException)Activator.CreateInstance(exceptionType, message)!;
            _otpFacadeMock.Setup(otpFacade => otpFacade.LoginUserWithOtpAsync(loginRequest))
                .ThrowsAsync(expectedException);

            await Assert.ThrowsAsync(exceptionType, () => _authController.LoginUserWithOtp(loginRequest));
        }

        [Fact]
        public async Task LoginWithOtp_OtpIsValid_ReturnsOkActionResult()
        {
            var loginRequest = new LoginWithOtpRequest();
            var jwtToken = "token";
            _otpFacadeMock.Setup(otpFacade => otpFacade.LoginUserWithOtpAsync(loginRequest))
                .ReturnsAsync(jwtToken);

            var actionResult = await _authController.LoginUserWithOtp(loginRequest);

            Assert.NotNull(actionResult);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(jwtToken, okResult.Value);
        }
    }
}

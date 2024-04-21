using Microsoft.AspNetCore.Http;
using Moq;
using OtpServer.Exception;
using OtpServer.Facade;
using OtpServer.Mapper;
using OtpServer.Mapper.Hash;
using OtpServer.Otp;
using OtpServer.Repository.Model;
using OtpServer.Security.Jwt;
using OtpServer.Service;
using System.Security.Claims;
using OtpServer.Dto;
using OtpServer.Request;

namespace OtpServer.Tests.Unit.Facade
{
    public class OtpFacadeTest
    {
        private readonly IOtpFacade _otpFacade;
        private readonly Mock<IOtpItemService> _otpItemServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJwtTokenHandler> _jwtTokenHandlerMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IOtpProvider> _otpProviderMock;
        private readonly Mock<IOtpItemMapper> _otpItemMapperMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public OtpFacadeTest()
        {
            _otpItemServiceMock = new Mock<IOtpItemService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtTokenHandlerMock = new Mock<IJwtTokenHandler>();
            _userServiceMock = new Mock<IUserService>();
            _otpProviderMock = new Mock<IOtpProvider>();
            _otpItemMapperMock = new Mock<IOtpItemMapper>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _otpFacade = new OtpFacade(
                _otpItemServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtTokenHandlerMock.Object,
                _userServiceMock.Object,
                _otpProviderMock.Object,
                _otpItemMapperMock.Object,
                _passwordHasherMock.Object
            );
        }

        [Fact]
        public async Task GenerateOtpAsync_LoggedInUserNotFound_ThrowsUserNotFoundException()
        {
            MockLoggedInUserNotFound();

            await Assert.ThrowsAsync<UserNotFoundException>(() => _otpFacade.GenerateOtpAsync());
        }

        [Fact]
        public async Task GenerateOtpAsync_FoundLoggedInUser_ReturnsSavedOtpItemMappedToDto()
        {
            var user = new User() { Uid = Guid.NewGuid() };
            var otp = "otp";
            var hashedOtp = "hashed";
            var expectedExpirationDate = DateTime.Now.AddMinutes(5);
            MockGetContextUserUid(user.Uid);
            _userServiceMock.Setup(service => service.GetUserByUidAsync(user.Uid))
                .ReturnsAsync(user);
            MockGenerateTotp(otp, expectedExpirationDate);
            _passwordHasherMock.Setup(hasher => hasher.HashPassword(otp))
                .Returns(hashedOtp);
            var otpItem = new OtpItem();
            _otpItemMapperMock.Setup(mapper => mapper.MapToPendingOtpItem(user, hashedOtp, expectedExpirationDate))
                .Returns(otpItem);
            var savedOtpItem = new OtpItem();
            _otpItemServiceMock.Setup(service => service.AddOtpItemAsync(otpItem))
                .ReturnsAsync(savedOtpItem);
            var savedOtpItemDto = new OtpItemDto();
            _otpItemMapperMock.Setup(mapper => mapper.MapToDto(savedOtpItem))
                .Returns(savedOtpItemDto);

            var result =  await _otpFacade.GenerateOtpAsync();

            Assert.Same(savedOtpItemDto, result);
        }

        [Fact]
        public async Task LoginUserWithOtpAsync_LoggedInUserNotFound_ThrowsUserNotFoundException()
        {
            var loginOtpRequest = new LoginWithOtpRequest();
            MockLoggedInUserNotFound();

            await Assert.ThrowsAsync<UserNotFoundException>(() => _otpFacade.LoginUserWithOtpAsync(loginOtpRequest));
        }

        [Fact]
        public async Task LoginUserWithOtpAsync_OtpIsInvalid_ThrowsInvalidOtpException()
        {
            var loginOtpRequest = new LoginWithOtpRequest { Otp = "otp" };
            MockGetOtpItemThrows(new InvalidOtpException("message"), loginOtpRequest.Otp);

            await Assert.ThrowsAsync<InvalidOtpException>(() => _otpFacade.LoginUserWithOtpAsync(loginOtpRequest));
        }

        [Fact]
        public async Task LoginUserWithOtpAsync_OtpIsExpired_ThrowsExpiredOtpException()
        {
            var loginOtpRequest = new LoginWithOtpRequest { Otp = "otp" };
            MockGetOtpItemThrows(new ExpiredOtpException("message"), loginOtpRequest.Otp);

            await Assert.ThrowsAsync<ExpiredOtpException>(() => _otpFacade.LoginUserWithOtpAsync(loginOtpRequest));
        }

        [Fact]
        public async Task LoginUserWithOtpAsync_OtpIsConsumed_ThrowsConsumedOtpException()
        {
            var loginOtpRequest = new LoginWithOtpRequest { Otp = "otp" };
            MockGetOtpItemThrows(new ConsumedOtpException("message"), loginOtpRequest.Otp);

            await Assert.ThrowsAsync<ConsumedOtpException>(() => _otpFacade.LoginUserWithOtpAsync(loginOtpRequest));
        }

        [Fact]
        public async Task LoginUserWithOtpAsync_OtpIsValid_ReturnsJwtToken()
        {
            var user = new User { Id = 1, Uid = Guid.NewGuid() };
            var otp = "otp";
            var loginOtpRequest = new LoginWithOtpRequest { Otp = otp };
            var otpItem = new OtpItem { Otp = otp };
            var token = "token";
            MockGetContextUserUid(user.Uid);
            _userServiceMock.Setup(userService => userService.GetUserByUidAsync(user.Uid))
                .ReturnsAsync(user);
            _otpItemServiceMock.Setup(otpItemService => otpItemService.GetOtpItemByUserIdAndOtpAsync(user.Id, otp))
                .ReturnsAsync(otpItem);
            _jwtTokenHandlerMock.Setup(handler => handler.GenerateJwtToken(user, TokenType.OtpLogin))
                .Returns(token);

            var result = await _otpFacade.LoginUserWithOtpAsync(loginOtpRequest);

            Assert.Same(token, result);
            _otpItemServiceMock.Verify(otpItemService => otpItemService.UpdateOtpItemAsync(otpItem), Times.Once);
            _jwtTokenHandlerMock.Verify(handler => handler.GenerateJwtToken(user, TokenType.OtpLogin), Times.Once);
        }

        private void MockGetOtpItemThrows(ApiException exception, string otp)
        {
            var user = new User { Id = 1, Uid = Guid.NewGuid() };
            MockGetContextUserUid(user.Uid);
            _userServiceMock.Setup(userService => userService.GetUserByUidAsync(user.Uid))
                .ReturnsAsync(user);
            _otpItemServiceMock.Setup(otpService => otpService.GetOtpItemByUserIdAndOtpAsync(user.Id, otp))
                .ThrowsAsync(exception);
        }

        private void MockLoggedInUserNotFound()
        {
            var userUid = Guid.NewGuid();
            MockGetContextUserUid(userUid);
            _userServiceMock.Setup(service => service.GetUserByUidAsync(userUid))
                .ThrowsAsync(new UserNotFoundException("message"));
        }

        private void MockGetContextUserUid(Guid userUid)
        {
            var mockHttpContext = new Mock<HttpContext>();
            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockHttpContext.SetupGet(context => context.User)
                .Returns(mockClaimsPrincipal.Object);
            _httpContextAccessorMock.Setup(accessor => accessor.HttpContext)
                .Returns(mockHttpContext.Object);
            _jwtTokenHandlerMock.Setup(handler => handler.FindClaim(JwtClaims.UserUid, mockClaimsPrincipal.Object))
                .Returns(userUid.ToString());
        }

        private void MockGenerateTotp(string otp, DateTime expirationDate)
        {
            _otpProviderMock.Setup(otpProvider => otpProvider.GenerateTotp(out It.Ref<DateTime>.IsAny))
                .Returns(otp)
                .Callback((out DateTime date) =>
                {
                    date = expirationDate;
                });
        }
    }
}

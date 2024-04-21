using Moq;
using OtpServer.Dto;
using OtpServer.Exception;
using OtpServer.Facade;
using OtpServer.Mapper;
using OtpServer.Repository.Model;
using OtpServer.Request;
using OtpServer.Security.Jwt;
using OtpServer.Service;

namespace OtpServer.Tests.Unit.Facade
{
    public class UserFacadeTest
    {
        private readonly IUserFacade _userFacade;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IUserMapper> _userMapperMock;
        private readonly Mock<IJwtTokenHandler> _jwtTokenHandlerMock;

        public UserFacadeTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _userMapperMock = new Mock<IUserMapper>();
            _jwtTokenHandlerMock = new Mock<IJwtTokenHandler>();
            _userFacade = new UserFacade(_userServiceMock.Object, _userMapperMock.Object, _jwtTokenHandlerMock.Object);
        }

        [Fact]
        public async Task AddUserAsync_ReturnsSavedUserMappedToDto()
        {
            var createUserRequest = GetMockCreateUserRequest();
            var user = GetMockUser();
            var savedUser = GetMockUser();
            var userDto = GetMockUserDto();
            _userMapperMock.Setup(mapper => mapper.ToUser(createUserRequest))
                .Returns(user);
            _userServiceMock.Setup(service => service.AddUserAsync(user))
                .ReturnsAsync(savedUser);
            _userMapperMock.Setup(service => service.ToDto(savedUser))
                .Returns(userDto);

            var result = await _userFacade.AddUserAsync(createUserRequest);

            Assert.Same(userDto, result);
        }

        [Fact]
        public async Task LoginUserAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            var loginRequest = GetMockLoginRequest();
            _userServiceMock.Setup(service => service.GetUserByUsernameAndPasswordAsync(loginRequest.Username, loginRequest.Password))
                .ThrowsAsync(new UserNotFoundException("message"));

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userFacade.LoginUserAsync(loginRequest));
        }

        [Fact]
        public async Task LoginUserAsync_UserWasFound_ReturnsJwtToken()
        {
            var loginRequest = GetMockLoginRequest();
            var foundUser = GetMockUser();
            var token = "token";
            _userServiceMock.Setup(service => service.GetUserByUsernameAndPasswordAsync(loginRequest.Username, loginRequest.Password))
                .ReturnsAsync(foundUser);
            _jwtTokenHandlerMock.Setup(handler => handler.GenerateJwtToken(foundUser, TokenType.MainLogin))
                .Returns(token);

            var result = await _userFacade.LoginUserAsync(loginRequest);

            Assert.Same(token, result);
        }

        private CreateUserRequest GetMockCreateUserRequest()
        {
            return new CreateUserRequest
            {
                Username = "username",
                Password = "password",
                Email = "email"
            };
        }

        private User GetMockUser()
        {
            return new User
            {
                Id = 1,
                Uid = Guid.NewGuid(),
                Username = "username",
                Password = "password",
                Email = "email"
            };
        }

        private UserDto GetMockUserDto()
        {
            return new UserDto
            {
                Uid = Guid.NewGuid(),
                Username = "username",
                Email = "email"
            };
        }

        private LoginRequest GetMockLoginRequest()
        {
            return new LoginRequest
            {
                Username = "username",
                Password = "password",
            };
        }
    }
}

using Moq;
using OtpServer.Mapper;
using OtpServer.Mapper.Hash;
using OtpServer.Repository.Model;
using OtpServer.Request;

namespace OtpServer.Tests.Unit.Mapper
{
    public class UserMapperTest
    {
        private readonly UserMapper _userMapper;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public UserMapperTest()
        {
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _userMapper = new UserMapper(_passwordHasherMock.Object);
        }

        [Fact]
        public void ToDto_MapsUserToUserDto()
        {
            var user = new User
            {
                Uid = Guid.NewGuid(),
                Username = "test_user",
                Email = "test@example.com"
            };

            var userDto = _userMapper.ToDto(user);

            Assert.Equal(user.Uid, userDto.Uid);
            Assert.Equal(user.Username, userDto.Username);
            Assert.Equal(user.Email, userDto.Email);
        }

        [Fact]
        public void ToUser_MapsCreateUserRequestToUser()
        {
            var request = new CreateUserRequest
            {
                Username = "test_user",
                Password = "test_password",
                Email = "test@example.com"
            };

            var expectedHashedPassword = "hashed_password";
            _passwordHasherMock.Setup(hasher => hasher.HashPassword(request.Password))
                .Returns(expectedHashedPassword);

            var user = _userMapper.ToUser(request);

            Assert.NotEqual(Guid.Empty, user.Uid); // Ensure a UID is assigned
            Assert.Equal(request.Username, user.Username);
            Assert.Equal(expectedHashedPassword, user.Password); // Check hashed password
            Assert.Equal(request.Email, user.Email);
        }
    }
}

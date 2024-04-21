using Moq;
using OtpServer.Exception;
using OtpServer.Mapper.Hash;
using OtpServer.Repository;
using OtpServer.Repository.Model;
using OtpServer.Service;

namespace OtpServer.Tests.Unit.Service
{
    public class UserServiceTest
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task AddUserAsync_UserWithUsernameAlreadyExists_ThrowsUserAlreadyExistsException()
        {
            var user = GetMockUser();
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(user.Username))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(user.Email))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService.AddUserAsync(user));
        }

        [Fact]
        public async Task AddUserAsync_UserWithEmailAlreadyExists_ThrowsUserAlreadyExistsException()
        {
            var user = GetMockUser();
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(user.Username))
                .ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(user.Email))
                .ReturnsAsync(user);

            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService.AddUserAsync(user));
        }

        [Fact]
        public async Task AddUserAsync_UserDoesNotExist_ReturnsSavedUser()
        {
            var user = GetMockUser();
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(user.Username))
                .ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(user.Email))
                .ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(repo => repo.AddAsync(user))
                .ReturnsAsync(() => user);

            var savedUser = await _userService.AddUserAsync(user);

            Assert.Same(user, savedUser);
        }

        [Fact]
        public async Task GetUserByUidAsync_UserDoesNotExist_ThrowsUserNotFoundException()
        {
            var uid = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetUserByUidAsync(uid))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserByUidAsync(uid));
        }

        [Fact]
        public async Task GetUserByUidAsync_UserExists_ReturnsFoundUser()
        {
            var user = GetMockUser();
            _userRepositoryMock.Setup(repo => repo.GetUserByUidAsync(user.Uid))
                .ReturnsAsync(user);

            var foundUser = await _userService.GetUserByUidAsync(user.Uid);

            Assert.Same(user, foundUser);
        }

        [Fact]
        public async Task GetUserByUsernameAndPasswordAsync_UserWithUsernameDoesNotExist_ThrowsUserNotFoundException()
        {
            var username = "username";
            var password = "password";
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserByUsernameAndPasswordAsync(username, password));
        }

        [Fact]
        public async Task GetUserByUsernameAndPasswordAsync_FoundUserPasswordDoesNotMatch_ThrowsUserNotFoundException()
        {
            var username = "username";
            var password = "password";
            var foundUser = GetMockUser();
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync(foundUser);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(password, foundUser.Password))
                .Returns(false);

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserByUsernameAndPasswordAsync(username, password));
        }


        [Fact]
        public async Task GetUserByUsernameAndPasswordAsync_FindsMatchingUser_ReturnsFoundUser()
        {
            var username = "username";
            var password = "password";
            var user = GetMockUser();
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(password, user.Password))
                .Returns(true);

            var foundUser = await _userService.GetUserByUsernameAndPasswordAsync(username, password);

            Assert.Same(user, foundUser);
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
    }
}

using OtpServer.Exception;
using OtpServer.Mapper.Hash;
using OtpServer.Repository;
using OtpServer.Repository.Model;

namespace OtpServer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AddUserAsync(User user)
        {
            bool userAlreadyExists = await _userRepository.GetUserByUsernameAsync(user.Username) != null ||
                await _userRepository.GetUserByEmailAsync(user.Email) != null;

            if (userAlreadyExists)
            {
                throw new UserAlreadyExistsException("User already exists.");
            }
            return await _userRepository.AddAsync(user);
        }

        public async Task<User> GetUserByUidAsync(Guid uid)
        {
            return await _userRepository.GetUserByUidAsync(uid) ?? throw new UserNotFoundException($"Could not find user with uid {uid}");
        }

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            User? user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                throw new UserNotFoundException($"Could not find user with username '{username}'");
            }
            if (!_passwordHasher.VerifyPassword(password, user.Password))
            {
                throw new UserNotFoundException($"Invalid password for user with username '{username}'");
            }
            return user;
        }
    }
}

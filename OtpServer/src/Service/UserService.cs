using OtpServer.Exception;
using OtpServer.Mapper.Hash;
using OtpServer.Repository;
using OtpServer.Repository.Model;

namespace OtpServer.Service
{
    public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<User> AddUserAsync(User user)
        {
            bool userAlreadyExists = await userRepository.GetUserByUsernameAsync(user.Username) != null ||
                await userRepository.GetUserByEmailAsync(user.Email) != null;

            if (userAlreadyExists)
            {
                throw new UserAlreadyExistsException("User already exists.");
            }
            return await userRepository.AddAsync(user);
        }

        public async Task<User> GetUserByUidAsync(Guid uid)
        {
            return await userRepository.GetUserByUidAsync(uid) ?? throw new UserNotFoundException($"Could not find user with uid {uid}");
        }

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            User? user = await userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                throw new UserNotFoundException($"Could not find user with username '{username}'");
            }
            if (!passwordHasher.VerifyPassword(password, user.Password))
            {
                throw new UserNotFoundException($"Invalid password for user with username '{username}'");
            }
            return user;
        }
    }
}

using OtpServer.Dto;
using OtpServer.Mapper.Hash;
using OtpServer.Repository.Model;
using OtpServer.Request;

namespace OtpServer.Mapper
{
    public class UserMapper : IUserMapper
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserMapper(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public UserDto ToDto(User user)
        {
            return new UserDto
            {
                Uid = user.Uid,
                Username = user.Username,
                Email = user.Email
            };
        }

        public User ToUser(CreateUserRequest request)
        {
            return new User
            {
                Uid = Guid.NewGuid(),
                Username = request.Username,
                Password = _passwordHasher.HashPassword(request.Password),
                Email = request.Email,
            };
        }
    }
}

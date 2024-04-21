using OtpServer.Dto;
using OtpServer.Repository.Model;
using OtpServer.Request;

namespace OtpServer.Mapper
{
    public interface IUserMapper
    {
        User ToUser(CreateUserRequest request);

        UserDto ToDto(User user);
    }
}

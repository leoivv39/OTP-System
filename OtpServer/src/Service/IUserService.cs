using OtpServer.Repository.Model;

namespace OtpServer.Service
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);

        Task<User> GetUserByUsernameAndPasswordAsync(string username, string password);

        Task<User> GetUserByUidAsync(Guid uid);
    }
}

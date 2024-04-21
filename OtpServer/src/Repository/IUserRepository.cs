using OtpServer.Repository.Model;

namespace OtpServer.Repository
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<User?> GetUserByUsernameAsync(string username);

        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByUidAsync(Guid uid);
    }
}

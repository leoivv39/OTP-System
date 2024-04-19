using OtpServer.Repository.Model;

namespace OtpServer.Repository
{
    public interface IOtpItemRepository : IRepository<OtpItem, int>
    {
        Task<List<OtpItem>> GetAllByUserIdAsync(int userId);
    }
}

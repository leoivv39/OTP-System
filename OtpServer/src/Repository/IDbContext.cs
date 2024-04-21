using Microsoft.EntityFrameworkCore;

namespace OtpServer.Repository
{
    public interface IDbContext<T> where T : class
    {
        DbSet<T> Set();

        Task<int> SaveChangesAsync();
    }
}

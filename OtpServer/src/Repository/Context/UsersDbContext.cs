using Microsoft.EntityFrameworkCore;
using OtpServer.Repository.Model;

namespace OtpServer.Repository.Context
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IDbContext<User>
    {
        public DbSet<User> Users { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public DbSet<User> Set()
        {

            return Users;
        }
    }
}

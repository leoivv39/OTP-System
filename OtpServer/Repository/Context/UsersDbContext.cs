using Microsoft.EntityFrameworkCore;
using OtpServer.Repository.Model;

namespace OtpServer.Repository.Context
{
    public class UsersDbContext : DbContext, IDbContext<User>
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) 
        { 
        }

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

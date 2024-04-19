using Microsoft.EntityFrameworkCore;
using OtpServer.Repository.Model;

namespace OtpServer.Repository.Context
{
    public class OtpItemDbContext : DbContext, IDbContext<OtpItem>
    {
        public OtpItemDbContext(DbContextOptions<OtpItemDbContext> options) : base(options)
        {
        }

        public DbSet<OtpItem> OtpItems { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public DbSet<OtpItem> Set()
        {

            return OtpItems;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OtpItem>()
                .Property(o => o.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OtpStatus)Enum.Parse(typeof(OtpStatus), v));
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OtpServer.Repository.Model;

namespace OtpServer.Repository
{
    public class OtpItemRepository : EfCoreRepository<OtpItem, int>, IOtpItemRepository
    {
        public OtpItemRepository(IDbContext<OtpItem> context) : base(context)
        {
        }

        public override async Task<OtpItem> AddAsync(OtpItem otpItem)
        {
            await base.AddAsync(otpItem);
            var savedEntity = await _dbSet
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == otpItem.Id);
            return savedEntity;
        }

        public async Task<List<OtpItem>> GetAllByUserIdAsync(int userId)
        {
            return await _dbSet.Where(otpItem => otpItem.UserId == userId)
                .ToListAsync();
        }
    }
}

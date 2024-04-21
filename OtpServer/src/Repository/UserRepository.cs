using Microsoft.EntityFrameworkCore;
using OtpServer.Repository.Model;
using System.Linq.Expressions;

namespace OtpServer.Repository
{
    public class UserRepository : EfCoreRepository<User, int>, IUserRepository
    {
        public UserRepository(IDbContext<User> context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await GetUserBy(user => user.Email.Equals(email));
        }

        public async Task<User?> GetUserByUidAsync(Guid uid)
        {
            return await GetUserBy(user => user.Uid.Equals(uid));
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await GetUserBy(user => user.Username.Equals(username));
        }

        private async Task<User?> GetUserBy(Expression<Func<User, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}

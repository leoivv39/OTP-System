using OtpServer.Repository.Model;

namespace OtpServer.Repository
{
    public interface IRepository<TEntity, TId> where TEntity : class, IEntity<TId>
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity?> GetByIdAsync(TId id);

        Task<TEntity?> DeleteByIdAsync(TId id);

        Task<List<TEntity>> GetAllAsync();
    }
}

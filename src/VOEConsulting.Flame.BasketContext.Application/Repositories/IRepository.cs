namespace VOEConsulting.Flame.BasketContext.Application.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<bool> IsExistsAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity,CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
    }

}

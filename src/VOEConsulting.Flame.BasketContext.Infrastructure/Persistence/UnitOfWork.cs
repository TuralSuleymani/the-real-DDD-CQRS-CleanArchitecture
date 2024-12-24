using VOEConsulting.Flame.BasketContext.Application.Repositories;
using VOEConsulting.Infrastructure.Persistence;

public class UnitOfWork(BasketAppDbContext dbContext) : IUnitOfWork
{
    private readonly BasketAppDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Save changes to the database
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _dbContext.Dispose();
}

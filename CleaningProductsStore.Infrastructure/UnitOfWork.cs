using CleaningProductsStore.Domain.Interfaces;
using CleaningProductsStore.Infrastructure.Contexts;

namespace CleaningProductsStore.Infrastructure;

public class UnitOfWork(CleaningProductsStoreContext dbContext) : IUnitOfWork
{
    private readonly CleaningProductsStoreContext _dbContext = dbContext;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

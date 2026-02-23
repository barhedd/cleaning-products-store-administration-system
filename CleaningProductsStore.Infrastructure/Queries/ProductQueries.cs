using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Application.Interfaces;
using CleaningProductsStore.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleaningProductsStore.Infrastructure.Queries;

public class ProductQueries(CleaningProductsStoreContext dbContext) : IProductQueries
{
    private readonly CleaningProductsStoreContext _dbContext = dbContext;

    public async Task<List<ProductByStatusDto>> GetByStatusAsync(bool isDeleted)
    {
        return await _dbContext.Set<ProductByStatusDto>()
            .FromSqlRaw("EXEC sp_GetProductsByStatus @IsDeleted = {0}", isDeleted)
            .ToListAsync();
    }
}

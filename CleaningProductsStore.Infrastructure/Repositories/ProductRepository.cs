using CleaningProductsStore.Domain.Entities;
using CleaningProductsStore.Domain.Interfaces;
using CleaningProductsStore.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleaningProductsStore.Infrastructure.Repositories;

public class ProductRepository(CleaningProductsStoreContext dbContext) : IProductRepository
{
    private readonly CleaningProductsStoreContext _dbContext = dbContext;
    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _dbContext.Products
            .AnyAsync(x => x.Code == code);
    }

    public async Task AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }
}

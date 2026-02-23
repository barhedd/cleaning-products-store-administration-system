using CleaningProductsStore.Domain.Entities;
using CleaningProductsStore.Domain.Interfaces;
using CleaningProductsStore.Infrastructure.Contexts;

namespace CleaningProductsStore.Infrastructure.Repositories;

public class OrderRepository(CleaningProductsStoreContext dbContext) : IOrderRepository
{
    private readonly CleaningProductsStoreContext _dbContext = dbContext;

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
    }
}

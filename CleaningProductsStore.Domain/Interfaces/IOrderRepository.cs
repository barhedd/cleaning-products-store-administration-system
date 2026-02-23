using CleaningProductsStore.Domain.Entities;

namespace CleaningProductsStore.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
}

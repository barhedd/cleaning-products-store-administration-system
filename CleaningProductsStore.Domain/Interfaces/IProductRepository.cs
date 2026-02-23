using CleaningProductsStore.Domain.Entities;

namespace CleaningProductsStore.Domain.Interfaces;

public interface IProductRepository
{
    Task<bool> ExistsByCodeAsync(string code);
    Task AddAsync(Product product);
}

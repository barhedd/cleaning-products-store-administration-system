using CleaningProductsStore.Application.DataTransferObjects;

namespace CleaningProductsStore.Application.Interfaces;

public interface IProductQueries
{
    Task<List<ProductByStatusDto>> GetByStatusAsync(bool isDeleted);
}

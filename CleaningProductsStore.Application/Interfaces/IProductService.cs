using CleaningProductsStore.Application.DataTransferObjects;

namespace CleaningProductsStore.Application.Interfaces;

public interface IProductService
{
    Task<Guid> CreateAsync(CreateProductRequestDTO request);
}

using CleaningProductsStore.Application.DataTransferObjects;

namespace CleaningProductsStore.Application.Interfaces;

public interface IOrderService
{
    Task<Guid> CreateAsync(CreateOrderRequestDto request);
}

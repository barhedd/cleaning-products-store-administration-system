using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Application.Interfaces;
using CleaningProductsStore.Domain.Entities;
using CleaningProductsStore.Domain.Interfaces;

namespace CleaningProductsStore.Application.Services;

public class OrderService(
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IOrderService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> CreateAsync(CreateOrderRequestDto request)
    {
        if (request.Items == null || request.Items.Count == 0)
            throw new ArgumentException("El pedido debe contener productos.");

        var productIds = request.Items
            .Select(x => x.ProductId)
            .ToList();

        var products = await _productRepository.GetByIdsAsync(productIds);

        if (products.Count != productIds.Count)
            throw new InvalidOperationException("Uno o más productos no existen.");

        var order = new Order
        {
            CreatedDate = DateTime.UtcNow,
        };

        foreach (var item in request.Items)
        {
            var product = products.First(x => x.Id == item.ProductId);

            if (product.Quantity < item.Quantity)
                throw new InvalidOperationException(
                    $"Stock insuficiente para el producto {product.Name}");
            
            // Discount inventory
            product.DecreaseStock(item.Quantity);

            order.AddItem(
                product.Id,
                product.Name,
                product.Price,
                item.Quantity);
        }

        order.CreatedDate = DateTimeOffset.UtcNow;

        await _orderRepository.AddAsync(order);

        await _unitOfWork.CommitAsync();

        return order.Id;
    }
}

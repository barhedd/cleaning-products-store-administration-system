using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Application.Interfaces;
using CleaningProductsStore.Domain.Entities;
using CleaningProductsStore.Domain.Interfaces;

namespace CleaningProductsStore.Application.Services;

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
{
    private readonly IProductRepository _repository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> CreateAsync(CreateProductRequestDTO request)
    {
        if (await _repository.ExistsByCodeAsync(request.Code))
            throw new InvalidOperationException("Ya existe un producto con ese código.");

        var product = new Product(
            request.Code,
            request.Name,
            request.Description,
            request.Price,
            request.Quantity
        )
        {
            CreatedDate = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(product);

        await _unitOfWork.CommitAsync();

        return product.Id;
    }
}

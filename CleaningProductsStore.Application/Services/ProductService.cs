using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Application.Interfaces;
using CleaningProductsStore.Domain.Entities;
using CleaningProductsStore.Domain.Interfaces;

namespace CleaningProductsStore.Application.Services;

public class ProductService(
    IProductRepository productRepository, 
    IProductQueries productQueries,
    IUnitOfWork unitOfWork) : IProductService
{
    private readonly IProductRepository _repository = productRepository;
    private readonly IProductQueries _queries = productQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> CreateAsync(CreateProductRequestDTO request)
    {
        if (await _repository.ExistsByCodeAsync(request.Code))
            throw new InvalidOperationException("Ya existe un producto con ese código.");

        var product = new Product
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(product);

        await _unitOfWork.CommitAsync();

        return product.Id;
    }

    public async Task<List<ProductByStatusDto>> GetByStatusAsync(bool isDeleted)
    {
        return await _queries.GetByStatusAsync(isDeleted);
    }
}

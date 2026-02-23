namespace CleaningProductsStore.Application.DataTransferObjects;

public record CreateProductRequestDTO
{
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public int Quantity { get; init; }
}

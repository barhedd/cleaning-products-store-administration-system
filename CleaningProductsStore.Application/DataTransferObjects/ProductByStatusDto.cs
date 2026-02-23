namespace CleaningProductsStore.Application.DataTransferObjects;

public record ProductByStatusDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
    public string CreatedBy { get; init; } = null!;
}

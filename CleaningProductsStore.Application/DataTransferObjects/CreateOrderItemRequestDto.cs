namespace CleaningProductsStore.Application.DataTransferObjects;

public record CreateOrderItemRequestDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}

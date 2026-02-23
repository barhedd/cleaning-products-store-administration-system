namespace CleaningProductsStore.Application.DataTransferObjects;

public record CreateOrderRequestDto
{
    public List<CreateOrderItemRequestDto> Items { get; set; } = [];
}

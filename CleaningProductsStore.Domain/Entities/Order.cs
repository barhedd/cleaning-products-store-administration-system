using CleaningProductsStore.Domain.Enums;

namespace CleaningProductsStore.Domain.Entities;

public class Order : BaseEntity
{
    public DateTimeOffset OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalAmount => _items.Sum(i => i.Total);

    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        _items.Add(new OrderItem(Guid.NewGuid(), productId, productName, unitPrice, quantity));
    }
}

namespace CleaningProductsStore.Domain.Entities;

public class Product() : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Cantidad inválida.");

        if (Quantity < quantity)
            throw new InvalidOperationException("Stock insuficiente.");

        Quantity -= quantity;
    }
}

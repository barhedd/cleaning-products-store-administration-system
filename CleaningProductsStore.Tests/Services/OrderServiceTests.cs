using CleaningProductsStore.Application.DataTransferObjects;
using CleaningProductsStore.Application.Services;
using CleaningProductsStore.Domain.Entities;
using CleaningProductsStore.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CleaningProductsStore.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
        _orderRepositoryMock = new Mock<IOrderRepository>(MockBehavior.Strict);
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);

        _service = new OrderService(
            _productRepositoryMock.Object,
            _orderRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldThrow_WhenNoItems()
    {
        var request = new CreateOrderRequestDto
        {
            Items = []
        };

        Func<Task> act = async () =>
            await _service.CreateAsync(request);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("*debe contener productos*");
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldThrow_WhenProductNotExists()
    {
        var productId = Guid.NewGuid();

        var request = new CreateOrderRequestDto
        {
            Items = [new() { ProductId = productId, Quantity = 2 }]
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync([]); // empty

        Func<Task> act = async () =>
            await _service.CreateAsync(request);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*no existen*");
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldThrow_WhenStockInsufficient()
    {
        var productId = Guid.NewGuid();

        var product = new Product
        {
            Code = "P001",
            Name = "Laptop",
            Description = "Description",
            Price = 1000,
            Quantity = 1
        };

        typeof(Product)
            .GetProperty("Id")!
            .SetValue(product, productId);

        var request = new CreateOrderRequestDto
        {
            Items = [new() { ProductId = productId, Quantity = 5 }]
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync([product]);

        Func<Task> act = async () =>
            await _service.CreateAsync(request);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*Stock insuficiente*");
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldCreateOrder_AndDecreaseStock()
    {
        var productId = Guid.NewGuid();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Code = "P001",
            Name = "Laptop",
            Description = "Description",
            Price = 1000,
            Quantity = 10
        };

        typeof(Product)
            .GetProperty("Id")!
            .SetValue(product, productId);

        var request = new CreateOrderRequestDto
        {
            Items = [new() { ProductId = productId, Quantity = 3 }]
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync([product]);

        _orderRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns((Task<int>)Task.CompletedTask);

        var orderId = await _service.CreateAsync(request);

        orderId.Should().NotBeEmpty();
        product.Quantity.Should().Be(7);

        _orderRepositoryMock.Verify(x =>
            x.AddAsync(It.IsAny<Order>()), Times.Once);

        _unitOfWorkMock.Verify(x =>
            x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

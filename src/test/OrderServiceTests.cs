using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

public class OrderServiceTests
{
    private AppDbContext CreateContext()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

        var context =
            new AppDbContext(options);
        if (!context.Products.Any())
        {
            context.Products.AddRange(
            [
                new Product
                {
                    Id = 1,
                    ProductName = "Mouse",
                    UnitPrice = 12.90m,
                    AvailableQuantity = 15
                },

                new Product
                {
                    Id = 2,
                    ProductName = "Keyboard",
                    UnitPrice = 169.90m,
                    AvailableQuantity = 10
                }
            ]);
        }
        context.SaveChanges();
        return context;
    }
    [Fact]
    public async Task ShouldNotConfirmWhenStockIsInsufficient()
    {
        Console.WriteLine("ShouldNotConfirmWhenStockIsInsufficient");
        var db = CreateContext();

        await db.SaveChangesAsync();

        var service = new OrderService(db);

        var order = await service.CreateOrder(
                new CreateOrderRequest
                {
                    CustomerId = "1",
                    Currency = "BRL",
                    Items =
                    [
                        new(1,12)
                    ]
                });
        var order2 = await service.CreateOrder(
                new CreateOrderRequest
                {
                    CustomerId = "1",
                    Currency = "BRL",
                    Items =
                    [
                        new(1,10)
                    ]
                });

        var confirmOrder2 = await service.CofirmOrder(order2.order.Id);

        var confirmOrder = await service.CofirmOrder(order.order.Id);

        Assert.True(confirmOrder2.Approved);
        Assert.False(confirmOrder.Approved);
    }
    [Fact]
    public async Task ShouldCreateOrder()
    {
        var db = CreateContext();

        var service = new OrderService(db);

        var request = new CreateOrderRequest
        {
            CustomerId = "12345",
            Currency = "BRL",
            Items =
                [
                    new(1,2)
                ]
        };

        var result = await service.CreateOrder(request);

        Assert.True(result.Approved);

        Assert.NotNull(result.order);

        Assert.Equal(OrderStatus.Placed, result.order.Status);
    }
    [Fact]
    public async Task ShouldNotCreateOrderWithoutItems()
    {
        var db = CreateContext();

        var service = new OrderService(db);

        var request = new CreateOrderRequest
        {
            CustomerId = "12345",
            Currency = "BRL",
            Items = []
        };
        var p = db.Products.First();

        var qtd = p.AvailableQuantity;


        var result = await service.CreateOrder(request);

        Assert.False(result.Approved);
    }
    [Fact]
    public async Task ShouldRejectInvalidProduct()
    {
        var db = CreateContext();

        var service = new OrderService(db);

        var request = new CreateOrderRequest
        {
            CustomerId = "12345",
            Currency = "BRL",
            Items =
                [
                    new(9999,1)
                ]
        };

        var result = await service.CreateOrder(request);

        Assert.False(result.Approved);

        Assert.Contains("Product", result.Message);
    }
    [Fact]
    public async Task ShouldConfirmOrder()
    {
        var db = CreateContext();

        var service = new OrderService(db);

        var p = db.Products.First();

        var qtd = p.AvailableQuantity;


        var order = await service.CreateOrder(
                new CreateOrderRequest
                {
                    CustomerId = "123",
                    Currency = "BRL",
                    Items =
                    [
                        new(1,2)
                    ]
                });

        var result = await service.CofirmOrder(order.order.Id);

        Assert.True(result.Approved);

        var product = db.Products.First(x => x.Id == 1);


        Assert.Equal(13, product.AvailableQuantity);
    }
    [Fact]
    public async Task ConfirmShouldBeIdempotent()
    {
        var db = CreateContext();

        var service = new OrderService(db);

        var order = await service.CreateOrder(
                new CreateOrderRequest
                {
                    CustomerId = "1",
                    Currency = "BRL",
                    Items =
                    [
                        new(1,1)
                    ]
                });

        await service.CofirmOrder(order.order.Id);

        var second = await service.CofirmOrder(order.order.Id);

        Assert.True(second.Approved);
    }
}
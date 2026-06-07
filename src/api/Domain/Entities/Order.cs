using System.Runtime.CompilerServices;

public class Order
{

    public Order()
    {
        Itens = new();
    }
    public Order(string nome, OrderStatus stt, string cur, List<OrderItem> ites)
    {
        CustomerId = nome;
        Status = stt;
        Currency = cur;
        Itens = ites;
        Total = Itens.Sum(x => x.Quantity * x.UnitPrice);
        CreatedAt = DateTime.Now;
    }

    public int Id { get; set; }
    public string CustomerId { get; set; } = String.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Draft;

    public String Currency { get; set; } = "BRL";
    public List<OrderItem> Itens { get; set; } = new List<OrderItem>();
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrderItem
{
    public OrderItem() { }
    public OrderItem(int prodId, decimal price, int qtd)
    {
        ProductId = prodId;
        UnitPrice = price;
        Quantity = qtd;
    }
    public OrderItem(int id, int prodId, decimal price, int qtd)
    {
        Id = id;
        ProductId = prodId;
        UnitPrice = price;
        Quantity = qtd;
    }
    public int? Id { get; set; }
    public int? OrderID { get; set; }
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}

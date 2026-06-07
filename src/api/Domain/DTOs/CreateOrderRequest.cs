public class CreateOrderRequest
{
    public string CustomerId { get; set; } = "";

    public string Currency { get; set; } = "";

    public List<CreateOrderItemRequest> Items
    { get; set; } = [];
}

public class CreateOrderItemRequest
{
    public CreateOrderItemRequest(int prodId, int qtd)
    {
        ProductId = prodId;
        Quantity = qtd;

    }
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
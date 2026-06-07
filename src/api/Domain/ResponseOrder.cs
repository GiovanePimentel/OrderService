public class ResponseOrder
{
    public Boolean Approved { get; set; }

    public Order? order { get; set; }

    public string? Message { get; set; }

}
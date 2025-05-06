
public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public List<OrderItem> Items { get; set; } = new();
}

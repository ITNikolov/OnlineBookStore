using OnlineBookStore.Models;
using System.Text.Json.Serialization;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Product? Product { get; set; }

    [JsonIgnore]
    public Order? Order { get; set; }
}

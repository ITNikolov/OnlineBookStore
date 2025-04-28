namespace OnlineBookStore.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Count { get; set; }

        public Product Product { get; set; }

    }
}
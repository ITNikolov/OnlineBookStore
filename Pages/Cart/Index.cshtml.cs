using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Utility;
using OnlineBookStore.Models;
using OnlineBookStore.Data; 

namespace OnlineBookStore.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db; 

        public List<CartItem> CartItems { get; set; } = new();

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            CartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            foreach (var item in CartItems)
            {
                item.Product = _db.Products.FirstOrDefault(p => p.Id == item.ProductId);
            }
        }
    }
}

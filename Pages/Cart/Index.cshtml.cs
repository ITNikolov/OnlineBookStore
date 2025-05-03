using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Utility;
using OnlineBookStore.Models;
using OnlineBookStore.Data;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> OnPostUpdateQuantityAsync(int id, string operation)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new();

            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                if (operation == "increase")
                    item.Quantity++;
                else if (operation == "decrease" && item.Quantity > 1)
                    item.Quantity--;
            }

            HttpContext.Session.Set("Cart", cart);
            return RedirectToPage();
        }


        public void OnGet()
        {
            CartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            foreach (var item in CartItems)
            {
                item.Product = _db.Products.FirstOrDefault(p => p.Id == item.ProductId);
            }
        }
        public IActionResult OnPostPlaceOrder()
        {
            HttpContext.Session.Remove("Cart"); 
            TempData["success"] = "Fake order placed. Cart cleared!";
            return RedirectToPage();
        }

    }
}

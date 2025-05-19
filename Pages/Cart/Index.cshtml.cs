using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Utility;

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
            LoadCartFromSession();
        }

        public IActionResult OnPostPlaceOrder()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToPage();
        }

        public IActionResult OnPostRemove(int id)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new();
            cart.RemoveAll(ci => ci.ProductId == id);
            HttpContext.Session.Set("Cart", cart);
            return RedirectToPage();
        }

        private void LoadCartFromSession()
        {
            CartItems = HttpContext.Session.Get<List<CartItem>>("Cart")
                        ?? new List<CartItem>();

            foreach (var item in CartItems)
            {
                item.Product = _db.Products
                                  .FirstOrDefault(p => p.Id == item.ProductId);
            }
        }
    }
}

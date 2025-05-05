using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Models;
using OnlineBookStore.Utility;

namespace OnlineBookStore.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new();

        public decimal TotalAmount { get; set; }

        public void OnGet()
        {
            CartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new();

            foreach (var item in CartItems)
            {
                TotalAmount += (decimal)item.Product.ListPrice * item.Quantity;
            }
        }

        public IActionResult OnPost()
        {
            // Placeholder - we will later add real order processing
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Your order has been placed!";
            return RedirectToPage("/Home/Index");
        }
    }
}

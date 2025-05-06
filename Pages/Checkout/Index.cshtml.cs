using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Models;
using OnlineBookStore.Utility;

namespace OnlineBookStore.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public CheckoutViewModel CheckoutForm { get; set; } = new();

        public decimal TotalAmount { get; set; }

        public void OnGet()
        {
            CheckoutForm.CartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new();

            foreach (var item in CheckoutForm.CartItems)
            {
                TotalAmount += (decimal)item.Product.ListPrice * item.Quantity;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Your order has been placed!";
            return RedirectToPage("/Home/Index");
        }
    }
}

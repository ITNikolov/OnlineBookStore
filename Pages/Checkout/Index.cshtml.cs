using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Utility;

namespace OnlineBookStore.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public CheckoutViewModel CheckoutForm { get; set; } = new();

        public decimal TotalAmount { get; set; }

        public void OnGet()
        {
            LoadCartAndCalculateTotal();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadCartAndCalculateTotal();
                return Page();
            }

            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Your order has been placed!";
            return RedirectToPage("/Home/Index");
        }

        private void LoadCartAndCalculateTotal()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new();
            foreach (var item in cart)
            {
                item.Product = _db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (item.Product != null)
                {
                    TotalAmount += (decimal)item.Product.ListPrice * item.Quantity;
                }
            }

            CheckoutForm.CartItems = cart;
        }
    }
}

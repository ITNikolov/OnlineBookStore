using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Utility;

namespace OnlineBookStore.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int productId, int quantity)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            HttpContext.Session.Set("Cart", cart);

            return RedirectToPage("/Cart/Index");
        }
    }
}

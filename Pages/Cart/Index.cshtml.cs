using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Utility;
using OnlineBookStore.Models;

namespace OnlineBookStore.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new();

        public void OnGet()
        {
            CartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
        }
    }
}

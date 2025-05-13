using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Utility;
using Order = OnlineBookStore.Models.Order;


namespace OnlineBookStore.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _users;
        public IndexModel(ApplicationDbContext db, UserManager<ApplicationUser> users)
        {
            _db = db;
            _users = users;
        }

        [BindProperty]
        public CheckoutViewModel CheckoutForm { get; set; } = new();

        public decimal TotalAmount { get; set; }

        public void OnGet()
        {
            LoadCartAndCalculateTotal();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCartAndCalculateTotal();
                return Page();
            }

            var cartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new();

            foreach (var item in cartItems)
            {
                item.Product = _db.Products.FirstOrDefault(p => p.Id == item.ProductId);
            }

            var currentUserId = _users.GetUserId(User);
            var order = new Models.Order
            {
                UserId = currentUserId,
                FullName = CheckoutForm.FullName,
                Email = CheckoutForm.Email,
                Address = CheckoutForm.Address,
                Country = CheckoutForm.Country,
                City = CheckoutForm.City,
                ZipCode = CheckoutForm.ZipCode,
                CardNumber = CheckoutForm.CardNumber,
                TotalAmount = cartItems.Sum(i => (decimal)i.Product.ListPrice * i.Quantity),
                Items = cartItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = (decimal)i.Product.ListPrice
                }).ToList()
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

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

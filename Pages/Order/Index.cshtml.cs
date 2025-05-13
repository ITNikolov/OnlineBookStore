using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Pages.Order
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _users;
        public List<Models.Order> Orders { get; set; } = new();

        public IndexModel(ApplicationDbContext db,UserManager<ApplicationUser> users)
        {
            _db = db;
            _users = users;
        }

        public async Task OnGetAsync()
        {
            var userId = _users.GetUserId(User);
            Console.WriteLine($"DEBUG Current userId = {userId}");

            Orders = await _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteOrderAsync(int orderId)
        {
            
            var userId = _users.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }

            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
                TempData["success"] = "Order deleted.";
            }

            return RedirectToPage();
        }

    }
}

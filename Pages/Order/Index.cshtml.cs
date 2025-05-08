using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Pages.Order
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Models.Order> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = User.Identity?.Name;
            if (!string.IsNullOrEmpty(userId))
            {
                Orders = await _db.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
        }
    }
}

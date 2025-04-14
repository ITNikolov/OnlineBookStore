using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Areas.Admin.Pages.Product
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<OnlineBookStore.Models.Product> Products { get; set; } = new List<OnlineBookStore.Models.Product>();

        public async Task OnGetAsync()
        {
            // Retrieve all products, including their Category details, sorted by Title.
            Products = await _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }
    }
}

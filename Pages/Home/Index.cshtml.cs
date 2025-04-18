using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
namespace OnlineBookStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Product> Featured { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            Featured = await _db.Products
                .Include(p => p.Category)      
                .OrderBy(p => p.Title)         
                .Take(25)                      
                .ToListAsync();
        }
    }
}

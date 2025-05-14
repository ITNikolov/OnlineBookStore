using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Utility;
namespace OnlineBookStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PaginatedList<Product> Featured { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            int pageSize = 10;
            var query = _db.Products
                .Include(j => j.Category)
                .OrderBy(j => j.Title)
                .AsNoTracking();

            Featured = await PaginatedList<Product>
                .CreateAsync(query, pageIndex ?? 1, pageSize);
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
namespace OnlineBookStore.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public const int PageSize = 10;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Product> Featured { get; set; } = new List<Product>();


        public async Task OnGetAsync(int pageNumber = 1)
        {
            var totalProducts = await _db.Products.CountAsync();
            TotalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);
            CurrentPage = pageNumber;

            Featured = await _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Title)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}

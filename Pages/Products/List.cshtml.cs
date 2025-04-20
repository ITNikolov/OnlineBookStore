using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineBookStore.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public ListModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Product> Products { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            Products = await _db.Products
                                .Include(p => p.Category)
                                .ToListAsync();
        }
    }
}

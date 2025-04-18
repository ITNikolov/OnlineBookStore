using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Areas.Admin.Pages.Product
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public IndexModel(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public List<OnlineBookStore.Models.Product> Products { get; set; } = new List<OnlineBookStore.Models.Product>();

        public async Task OnGetAsync()
        {
            Products = await _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            
            var prod = await _db.Products.FindAsync(id);
            if (prod != null)
            {
                if (!string.IsNullOrEmpty(prod.ImageURL))
                {
                    var filePath = Path.Combine(_env.WebRootPath,
                                                prod.ImageURL.TrimStart('\\', '/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _db.Products.Remove(prod);
                await _db.SaveChangesAsync();
                TempData["success"] = "Product deleted successfully";
            }

            return RedirectToPage();
        }
    }
}

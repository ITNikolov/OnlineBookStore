using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Areas.Admin.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<OnlineBookStore.Models.Category> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Retrieve categories from DB, ordered by DisplayOrder
            Categories = await _db.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        // This method handles deleting a category on POST
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            // Display a success message after deleting
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage();
        }
    }
}

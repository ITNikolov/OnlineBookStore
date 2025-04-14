using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Areas.Admin.Pages.Category
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public OnlineBookStore.Models.Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Category = await _db.Categories.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(Category);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category deleted successfully";

            return RedirectToPage("Index");
        }
    }
}

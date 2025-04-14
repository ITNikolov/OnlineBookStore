using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Areas.Admin.Pages.Category
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public EditModel(ApplicationDbContext db)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Attach(Category).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["success"] = "Category updated successfully";
            return RedirectToPage("Index");
        }

        private bool CategoryExists(int id)
        {
            return _db.Categories.Any(c => c.CategoryId == id);
        }
    }
}

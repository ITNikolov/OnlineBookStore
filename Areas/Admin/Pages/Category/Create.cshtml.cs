using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Data;

namespace OnlineBookStore.Areas.Admin.Pages.Category
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public OnlineBookStore.Models.Category Category { get; set; } = new OnlineBookStore.Models.Category();

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("OnPostAsync triggered!");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is INVALID:");
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"- {kvp.Key}: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            try
            {
                Console.WriteLine($"Saving Category: {Category.Name} | Display Order: {Category.DisplayOrder}");
                _db.Categories.Add(Category);
                await _db.SaveChangesAsync();
                Console.WriteLine("Successfully saved to DB");
                TempData["success"] = "Category created successfully";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving category: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the category.");
                return Page();
            }
        }
    }
}

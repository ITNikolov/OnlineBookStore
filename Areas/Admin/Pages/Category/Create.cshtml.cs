using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Threading.Tasks;

namespace OnlineBookStore.Areas.Admin.Pages.Category
{
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
            // This method handles the GET request to display the form.
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(Category);
                await _db.SaveChangesAsync();

                TempData["success"] = "Category created successfully";
                return RedirectToPage("Index"); // Redirect to Index.cshtml after successful creation
            }
            return Page();
        }
    }
}

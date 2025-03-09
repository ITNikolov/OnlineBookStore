using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Models;
using System.Collections.Generic;
using System.Linq;
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


        public void OnGet()
        {
            Categories = _db.Categories.OrderBy(c => c.DisplayOrder).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";

            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Areas.Admin.Pages.Product
{
    [Authorize(Roles = "Admin")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public UpsertModel(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public OnlineBookStore.Models.Product Product { get; set; } = new OnlineBookStore.Models.Product();

        // For the Category dropdown
        public SelectList CategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var categories = await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            CategoryList = new SelectList(categories, "CategoryId", "Name");

            if (id.HasValue)
            {
                Product = await _db.Products
                    .FirstOrDefaultAsync(p => p.Id == id.Value);

                if (Product == null)
                    return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cats = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            CategoryList = new SelectList(cats, "CategoryId", "Name");

            var file = Request.Form.Files.FirstOrDefault();

            if (Product.Id == 0 && (file == null || file.Length == 0))
            {
                ModelState.AddModelError(string.Empty, "Image is required.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            if (file != null && file.Length > 0)
            {
                var wwwRoot = _env.WebRootPath;
                var uploads = Path.Combine(wwwRoot, "images", "product");
                Directory.CreateDirectory(uploads); 

                if (!string.IsNullOrEmpty(Product.ImageURL))
                {
                    var oldImagePath = Path.Combine(wwwRoot, Product.ImageURL.TrimStart('\\', '/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Save relative path to DB
                Product.ImageURL = Path.Combine("images", "product", fileName).Replace('\\', '/');
            }

            if (Product.Id == 0)
            {
                _db.Products.Add(Product);
                TempData["success"] = "Product created successfully";
            }
            else
            {
                _db.Products.Update(Product);
                TempData["success"] = "Product updated successfully";
            }

            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }


    }
}

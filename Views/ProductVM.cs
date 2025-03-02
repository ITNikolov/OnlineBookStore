using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBookStore.Models;

namespace OnlineBookStore.Views
{
    public class ProductVM
    {
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public Product Product { get; set; }
    }

}

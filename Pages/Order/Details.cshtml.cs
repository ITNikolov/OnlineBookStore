using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineBookStore.Pages.Order
{
    public class DetailsModel : PageModel
    {
        public List<Models.Order> Orders { get; set; } = new();
        public void OnGet()
        {
        }
    }
}

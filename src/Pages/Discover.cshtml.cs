using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages
{
    public class DiscoverModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Discover Your Avenger";
        }
    }
}

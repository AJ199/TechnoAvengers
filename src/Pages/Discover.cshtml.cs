using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Handles the logic for displaying the Discover Razor Page
    /// </summary>
    public class DiscoverModel : PageModel
    {
        /// <summary>
        /// Handles GET requests to load the Discover page
        /// </summary>
        public void OnGet()
        {
            ViewData["Title"] = "Discover Your Avenger";
        }
    }
}
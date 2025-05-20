using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// PageModel for displaying comparison results between two heroes.
    /// </summary>
    public class CompareResultModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor that injects the product service.
        /// </summary>
        public CompareResultModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        public ProductModel Hero1 { get; set; }
        public ProductModel Hero2 { get; set; }

        /// <summary>
        /// Handles the GET request and loads the two selected heroes.
        /// Redirects back to Compare page if any hero is not found.
        /// </summary>
        /// <param name="hero1Id">ID of first hero</param>
        /// <param name="hero2Id">ID of second hero</param>
        /// <returns>IActionResult to render the page or redirect</returns>
        public IActionResult OnGet(string hero1Id, string hero2Id)
        {
            var all = _productService.GetProducts().ToList();

            Hero1 = all.FirstOrDefault(x => x.Id == hero1Id);
            Hero2 = all.FirstOrDefault(x => x.Id == hero2Id);

            if (Hero1 == null || Hero2 == null) return RedirectToPage("Compare");

            return Page();
        }
    }
}
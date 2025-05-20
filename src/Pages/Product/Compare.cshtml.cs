using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// CompareModel class for comparing two heroes.
    /// </summary>
    public class CompareModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor that injects the product service.
        /// </summary>
        /// <param name="productService">Service to access product data</param>
        public CompareModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// List of all products to be used in comparison.
        /// </summary>
        public List<ProductModel> Products { get; set; } = new();

        [BindProperty]
        public string? Hero1Id { get; set; }

        [BindProperty]
        public string? Hero2Id { get; set; }

        public List<SelectListItem> HeroOptions { get; set; } = new();

        /// <summary>
        /// Handles GET requests to populate hero list and dropdown options.
        /// </summary>
        public void OnGet()
        {
            Products = _productService.GetProducts().ToList();

            HeroOptions = Products.Select(hero => new SelectListItem
            {
                Value = hero.Id,
                Text = hero.Title
            }).ToList();
        }

        /// <summary>
        /// Handles POST requests to compare two heroes.
        /// Returns same page if validation fails, otherwise redirects to result.
        /// </summary>
        /// <returns>IActionResult to either reload or redirect</returns>
        public IActionResult OnPost()
        {
            if (Hero1Id == Hero2Id || string.IsNullOrEmpty(Hero1Id) || string.IsNullOrEmpty(Hero2Id))
            {
                Products = _productService.GetProducts().ToList();
                HeroOptions = Products.Select(hero => new SelectListItem
                {
                    Value = hero.Id,
                    Text = hero.Title
                }).ToList();

                return Page(); // Stay on page with options intact
            }

            // Redirect to comparison result page with both selected hero IDs
            return RedirectToPage("CompareResult", new { hero1Id = Hero1Id, hero2Id = Hero2Id });
        }
    }
}
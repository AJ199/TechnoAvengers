using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// PageModel for the Rank page, responsible for providing superhero data sorted by score
    /// </summary>
    public class RankModel : PageModel
    {
        // Injected JSON file product service for accessing superhero data
        private readonly JsonFileProductService ProductService;

        // Collection of superheroes to be displayed on the Rank page
        public IEnumerable<ProductModel> Superheroes { get; private set; }

        // Constructor injecting the product service
        public RankModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Handles GET requests to the Rank page, fetches and sorts superheroes by score
        /// </summary>
        public void OnGet()
        {
            Superheroes = ProductService.GetProducts()
                .OrderByDescending(p => p.Score); // Sort superheroes in descending order of score
        }
    }
}

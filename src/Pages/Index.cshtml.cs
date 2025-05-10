using System.Collections.Generic;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Mariana Marquez Arce
    /// Luke McClure!
    /// Anurag Jindal
    /// Suvathi Kannan
    /// </summary>
    public class IndexModel : PageModel
    {
        // Logger for debugging and diagnostics
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Initializes a new instance of the IndexModel class.
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="productService">Service for accessing superhero data</param>
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        /// <summary>
        /// Data service for accessing and filtering superhero records.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// All superheroes retrieved from the service.
        /// </summary>
        public IEnumerable<ContosoCrafts.WebSite.Models.ProductModel>? Products { get; private set; }

        /// <summary>
        /// Category used for filtering (e.g., Alignment, Role, Gender).
        /// Bound from the query string.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string FilterCategory { get; set; }

        /// <summary>
        /// Value within the selected category used for filtering.
        /// Bound from the query string.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string FilterValue { get; set; }

        /// <summary>
        /// Filtered list of superheroes based on the selected category and value.
        /// </summary>
        public List<ProductModel> FilteredHeroes { get; set; }

        /// <summary>
        /// Handles GET requests to the index page.
        /// Applies filtering logic based on FilterCategory and FilterValue query parameters.
        /// </summary>
        public void OnGet()
        {
            var allHeroes = ProductService.GetProducts();

            FilteredHeroes = allHeroes
                .Where(hero =>
                    string.IsNullOrEmpty(FilterCategory) || string.IsNullOrEmpty(FilterValue) ||
                    (FilterCategory == "Alignment" && hero.Alignment == FilterValue) ||
                    (FilterCategory == "Role" && hero.Role == FilterValue) ||
                    (FilterCategory == "Gender" && hero.Gender == FilterValue)
                ).ToList();
        }
    }
}
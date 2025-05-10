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
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        public JsonFileProductService ProductService { get; }
        public IEnumerable<ContosoCrafts.WebSite.Models.ProductModel>? Products { get; private set; }

        //public void OnGet() => Products = ProductService.GetProducts();

        [BindProperty(SupportsGet = true)]
        public string FilterCategory { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FilterValue { get; set; }

        public List<ProductModel> FilteredHeroes { get; set; }

        public void OnGet()
        {
            var allHeroes = ProductService.GetProducts();

            FilteredHeroes = allHeroes
                .Where(hero =>
                    string.IsNullOrEmpty(FilterCategory) || string.IsNullOrEmpty(FilterValue) ||
                    (FilterCategory == "Alignment" && hero.Alignment == FilterValue)
                ).ToList();
        }
    }
}
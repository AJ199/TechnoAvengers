using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;  // <-- Add this

public class Example
{
    public void Method(IEnumerable<ProductModel> products)
    {
        List<ProductModel> list = products.ToList(); // Now this works
    }
}


namespace ContosoCrafts.WebSite.Pages.Product
{
    public class BattleModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor that injects the product service.
        /// </summary>
        /// <param name="productService">Service to access product data</param>
        public BattleModel(JsonFileProductService productService)
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


    }
}

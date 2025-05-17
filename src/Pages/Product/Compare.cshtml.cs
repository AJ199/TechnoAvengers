using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CompareModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public CompareModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        public List<ProductModel> Products { get; set; } = new();
        public ProductModel? Hero1 { get; set; }
        public ProductModel? Hero2 { get; set; }

        [BindProperty]
        public string? Hero1Id { get; set; }

        [BindProperty]
        public string? Hero2Id { get; set; }

        public void OnGet()
        {
            Products = _productService.GetProducts().ToList();
        }

        public IActionResult OnPost()
        {
            if (Hero1Id == Hero2Id || string.IsNullOrEmpty(Hero1Id) || string.IsNullOrEmpty(Hero2Id))
            {
                return Page(); // Validation fallback
            }

            return RedirectToPage("CompareResult", new { hero1Id = Hero1Id, hero2Id = Hero2Id });
        }
    }
}

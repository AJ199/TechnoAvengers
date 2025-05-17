using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty]
        public string? Hero1Id { get; set; }

        [BindProperty]
        public string? Hero2Id { get; set; }

        public List<SelectListItem> HeroOptions { get; set; } = new();

        public void OnGet()
        {
            Products = _productService.GetProducts().ToList();

            HeroOptions = Products.Select(hero => new SelectListItem
            {
                Value = hero.Id,
                Text = hero.Title
            }).ToList();
        }

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

            return RedirectToPage("CompareResult", new { hero1Id = Hero1Id, hero2Id = Hero2Id });
        }
    }
}

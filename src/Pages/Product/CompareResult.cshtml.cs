using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CompareResultModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public CompareResultModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        public ProductModel Hero1 { get; set; }
        public ProductModel Hero2 { get; set; }

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

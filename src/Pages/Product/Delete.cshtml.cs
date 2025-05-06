using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public DeleteModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        public ProductModel? Product { get; private set; }

        public IActionResult OnGet()
        {
            Product = _productService.GetProducts().FirstOrDefault(p => p.Id == Id);
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var products = _productService.GetProducts().ToList();
            var product = products.FirstOrDefault(p => p.Id == Id);

            if (product != null)
            {
                products.Remove(product);
                _productService.SaveData(products);
                TempData["ShowPopup"] = true;
            }

            return RedirectToPage("/Product/Index");
        }
    }
}

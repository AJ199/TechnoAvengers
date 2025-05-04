using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ProductModel Product { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");

            var json = System.IO.File.ReadAllText(filePath);
            var products = JsonConvert.DeserializeObject<List<ProductModel>>(json) ?? new List<ProductModel>();

            // Generate a new unique ID (maybe need to improve this???)
            Product.Id = (products.Count + 1).ToString();

            products.Add(Product);

            System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(products, Formatting.Indented));

            // Redirect to the Read page with the new product's ID
            return RedirectToPage("Read", new { id = Product.Id });
        }
    }
}


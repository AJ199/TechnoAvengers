using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class IndexModel : PageModel
    {
        // List of products to be displayed on the page
        public List<ContosoCrafts.WebSite.Models.ProductModel> Products { get; set; }

        /// <summary>
        /// Handles GET requests to load product data from JSON
        /// </summary>
        public void OnGet()
        {
            // Build the full file path to the JSON file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");

            // Read the contents of the JSON file
            var json = System.IO.File.ReadAllText(filePath);

            // Deserialize the JSON into a list of ProductModel objects
            Products = JsonConvert.DeserializeObject<List<ContosoCrafts.WebSite.Models.ProductModel>>(json);
        }
    }
}
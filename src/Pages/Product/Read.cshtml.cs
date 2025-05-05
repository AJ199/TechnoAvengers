using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Page model for reading superhero data
    /// </summary>
    public class ReadModel : PageModel
    {
        // Service that accesses superhero data
        public JsonFileProductService ProductService { get; }

        // Stores superhero associated with provided ID
        public ProductModel? Product { get; set; }

        /// <summary>
        /// Initializes new instance of ReadModel with the given service
        /// </summary>
        /// <param name="productService">Service used to retrieve data</param>
        public ReadModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Responds to GET requests and loads the appropiate superhero 
        /// </summary>
        /// <param name="id">Superhero ID</param>
        public void OnGet(string id)
        // x.Id == productId
        {
            // Get the product using the id
            var retrievedProduct = ProductService.GetProducts().FirstOrDefault(m => m.Id == id);

            if (retrievedProduct == null)
            {
                RedirectToPage("/Error");
                return;
            }

            // Assigns the product only if it's not null
            Product = retrievedProduct;
        }
    }
}

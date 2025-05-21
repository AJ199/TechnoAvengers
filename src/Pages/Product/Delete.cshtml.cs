using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Handles the logic for deleting a product from the system.
    /// </summary>
    public class DeleteModel : PageModel
    {
        /// <summary>
        /// Provides access to the product data stored in JSON.
        /// </summary>
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor that initializes the data service.
        /// </summary>
        /// <param name="productService">Service used to manage product data operations</param>
        public DeleteModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// The ID of the product to be deleted, passed via query string.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        /// <summary>
        /// The product to display on the confirmation page before deletion.
        /// </summary>
        public ProductModel? Product { get; private set; }

        /// <summary>
        /// Handles GET requests to the Delete page.
        /// Loads the product to confirm deletion.
        /// </summary>
        /// <returns>
        /// Returns the Delete page if the product is found, otherwise redirects to the Index page.
        /// </returns>
        public IActionResult OnGet()
        {
            // Attempt to find the product with the specified ID
            Product = _productService.GetProducts().FirstOrDefault(p => p.Id == Id);

            // Redirect to index if product is not found
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Show the confirmation page
            return Page();
        }

        /// <summary>
        /// Handles POST requests to actually delete the product after confirmation.
        /// </summary>
        /// <returns>
        /// Redirects to the Index page after deletion.
        /// </returns>
        public IActionResult OnPost()
        {
            // Get the current list of products
            var products = _productService.GetProducts().ToList();

            // Find the product to delete
            var product = products.FirstOrDefault(p => p.Id == Id);

            // If product is not found, skip deletion
            if (product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Remove the product from the list
            products.Remove(product);

            // Save the updated product list
            _productService.SaveData(products);

            // Set flag to show popup confirmation on the index page
            TempData["ShowPopup"] = true;

            return RedirectToPage("/Product/Index");
        }
    }
}
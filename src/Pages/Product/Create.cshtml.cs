using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using System.Linq;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Manages the creation of a new product entry for the Create Razor Page.
    /// </summary>
    public class CreateModel : PageModel
    {
        /// <summary>
        /// Provides access to the product data stored in JSON.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Holds the new product data input by the user for binding to the form.
        /// </summary>
        [BindProperty]
        public ProductModel? Product { get; set; }

        /// <summary>
        /// Constructor that initializes the data service for use in this page.
        /// </summary>
        /// <param name="productService">Service used to manage product data operations</param>
        public CreateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Handles the GET request to load the Create page.
        /// </summary>
        /// <returns>The current page</returns>
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Handles the POST request when the form is submitted.
        /// Validates and saves the new product data.
        /// </summary>
        /// <returns>
        /// Redirects to the Read page if successful, returns the Create page on error.
        /// </returns>
        public IActionResult OnPost()
        {
            // Generate unique product ID
            Product.Id = GenerateUniqueProductId();

            // Return to the page if model validation fails
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            // Null check for safety
            if (Product == null)
            {
                return RedirectToPage("/Error");
            }

            // Generate ID again (possibly redundant)
            Product.Id = GenerateUniqueProductId();

            // Attempt to create and save the product
            bool result = ProductService.CreateData(Product);

            // Handle creation failure
            if (result == false)
            {
                ModelState.AddModelError("CreateFailure", "Failed to create product.");
                return Page();
            }

            // Redirect to the Read page for the newly created product
            return RedirectToPage("Read", new { id = Product.Id });
        }

        /// <summary>
        /// Generates a new unique product ID by incrementing the current maximum ID.
        /// Assumes all product IDs are numeric strings.
        /// </summary>
        /// <returns>A new unique product ID as a string</returns>
        private string GenerateUniqueProductId()
        {
            // Get all existing products
            var products = ProductService.GetProducts().ToList();

            // Track the maximum existing numeric ID
            int maxId = 0;

            foreach (var product in products)
            {
                int numericId = int.Parse(product.Id);

                if (numericId > maxId)
                {
                    maxId = numericId;
                }
            }

            // Return a new ID as string
            int newId = maxId + 1;
            return newId.ToString();
        }
    }
}

}

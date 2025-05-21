using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Manages update of data for update page
    /// </summary>
    public class UpdateModel : PageModel
    {
        // Data middletier service
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Initializes a new UpdateModel instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService">Service used to manage superhero data</param>
        public UpdateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // Stores the product information to be displayed and updated
        [BindProperty]
        public ProductModel? Product { get; set; }

        /// <summary>
        ///  Responds to GET requests and loads the appropiate superhero data
        ///  If no match is found, an error is added to the model state.
        /// </summary>
        /// <param name="id">Superhero ID</param>
        public void OnGet(string id)
        {
            // Retrieves the superhero associated with the given ID
            var retrievedProduct = ProductService.GetProducts().FirstOrDefault(m => m.Id == id);

            if (retrievedProduct == null)
            {
                this.ModelState.AddModelError("OnGet", "Update OnGet Error");
                
                return;
            }

            // Assigns the superhero data 
            Product = retrievedProduct;
        }

        /// <summary>
        /// Validates the model, updates the data and navigates to appropriate page based outcome
        /// </summary>
        /// <returns>
        /// Index page on successful update, 
        /// current page if validation fails, 
        /// Error page if product is null
        /// <returns>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            if (Product == null) 
            {
                return RedirectToPage("/Error");
            }

            // Attempt to update data, return error if update fails
            if (ProductService.UpdateData(Product) == false)
            {
                this.ModelState.AddModelError("UpdateFailure", "Failed to update product data.");
                
                return Page(); 
            }

            return RedirectToPage("/Product/Index");
        }
    }
}
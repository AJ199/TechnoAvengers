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
        /// Defualt Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService">Service used to manage superhero data</param>
        public UpdateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // The data to show, bind to it for the post
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        ///  Responds to GET requests and loads the appropiate superhero data
        /// </summary>
        /// <param name="id">Superhero ID</param>
        public void OnGet(string id)
        {
            // Retrieves the superhero associated with the given ID
            var retrievedProduct = ProductService.GetProducts().FirstOrDefault(m => m.Id.Equals(id));

            if (retrievedProduct == null)
            {
                this.ModelState.AddModelError("OnGet", "Update OnGet Error");
                
                return;
            }

            // Assigns the superhero data 
            Product = retrievedProduct;
        }

        /// <summary>
        /// Post the model back to the page
        /// The model is in the class variable Product
        /// Call the data layer to Update that data
        /// Then return to the index page
        /// </summary>
        /// <returns>Redirects to Index page if successful, else reloads current page</returns>
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

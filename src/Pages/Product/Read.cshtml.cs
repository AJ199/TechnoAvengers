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

        // Binds the {id} segment on GET and POST
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        // Stores superhero associated with provided ID
        public ProductModel? Product { get; set; }

        // Current average of ratings 
        public int CurrentRating { get; set; }

        // Vote count of ratings
        public int VoteCount { get; set; }

        // Label to be displayed with ratings
        public string VoteLabel { get; set; } = string.Empty;

        // Holds the response to be sent back to the client.
        public IActionResult? ClientResponse { get; set; }

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
        {
            // Get the product using the id
            var retrievedProduct = ProductService.GetProducts().FirstOrDefault(m => m.Id == id);

            if (retrievedProduct == null)
            {
                ClientResponse = RedirectToPage("/Error");
                return;
            }

            // Assigns the product only if it's not null
            Product = retrievedProduct;
        }

        /// <summary>
        /// Calculates VoteCount, VoteLabel and CurrentRating average
        /// </summary>
        public void CalculateRating()
        {
            // Retrieves the existent ratings, if empty, assigns an empty collection
            var ratings = Product.Ratings ?? Enumerable.Empty<int>();
            VoteCount = ratings.Count();
            VoteLabel = VoteCount == 1 ? "Vote" : "Votes";
            CurrentRating = VoteCount > 0 ? ratings.Sum() / VoteCount : 0;
        }
    }
}
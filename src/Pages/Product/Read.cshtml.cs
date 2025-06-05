using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Page model for reading superhero data, interaction 
    /// with comment submission and rating functionality
    /// </summary>
    public class ReadModel : PageModel
    {
        // Service that accesses superhero data
        public JsonFileProductService ProductService { get; }

        // Service to access comments
        public JsonFileCommentService CommentService;

        // Binds the {id} segment on GET and POST
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        // Stores superhero associated with provided ID
        public ProductModel? Product { get; set; }

        // Comment input from the user
        [BindProperty]
        public CommentModel NewComment { get; set; }

        // List of all comments associated with the superhero
        public List<CommentModel> Comments { get; set; }

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
        /// <param name="productService">Service used to reading data</param>
        /// <param name="commentService">Service used to reading/writing comments</param>
        public ReadModel(JsonFileProductService productService, JsonFileCommentService commentService)
        {
            ProductService = productService;
            CommentService = commentService;
        }

        /// <summary>
        /// Responds to GET requests, retrieves superhero, loads ratings and comments
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

            CalculateRating();

            Comments = CommentService.GetComments(Id);
            NewComment = new CommentModel
            {
                SuperheroId = Id
            };
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
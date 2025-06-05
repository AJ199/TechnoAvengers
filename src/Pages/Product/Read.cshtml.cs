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
        /// Submits a new comment
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <param name="message">Message to submit</param>
        /// <param name="superheroId">ID of superhero the comment is for</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddComment([FromForm] string username, [FromForm] string message, [FromForm] string superheroId)
        {
            // Comment object from form values
            var comment = new CommentModel
            {
                Username = username,
                Message = message,
                SuperheroId = superheroId
            };

            // Validation context for the comment model
            var validationContext = new ValidationContext(comment);

            // List of validation results
            var validationResults = new List<ValidationResult>();

            // Performs validation on the comment
            var isValid = Validator.TryValidateObject(comment, validationContext, validationResults, validateAllProperties: true);

            if (isValid == false)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = validationResults.Select(r => r.ErrorMessage)
                });
            }

            CommentService.AddComment(comment);

            // Retrieve updated list of comments
            var updatedComments = CommentService.GetComments(superheroId);

            return new JsonResult(new
            {
                success = true,
                comments = updatedComments.Select(c => new
                {
                    c.Username,
                    c.Message,
                    c.Likes,
                    c.Id
                })
            });
        }

        /// <summary>
        /// Submits a new rating and returns updated average
        /// </summary>
        /// <param name="id">Superhero ID</param>
        /// <param name="rating">New rating value (1-5)</param>
        /// <returns>JSON result with updated vote count and average</returns>
        public async Task<IActionResult> OnPostAddRating([FromForm] string id, [FromForm] int rating)
        {
            Id = id;
            ProductService.AddRating(Id, rating);

            // Re-fetch the updated product
            Product = ProductService.GetProducts().FirstOrDefault(p => p.Id == Id);

            if (Product == null)
            {
                return new JsonResult(new { error = "Product not found" });
            }

            // Retrieved ratings
            var (voteCount, average) = ComputeRatingStats(Product);

            // Count of votes
            return new JsonResult(new
            {
                voteCount,
                average
            });
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
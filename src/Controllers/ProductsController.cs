using System.Collections.Generic;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Controllers
{
    /// <summary>
    /// API controller that handles HTTP requests related to product data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Constructor that injects the JSON product service.
        /// </summary>
        /// <param name="productService">Service for managing product data.</param>
        public ProductsController(JsonFileProductService productService) =>
            ProductService = productService;

        /// <summary>
        /// Service for accessing and modifying product data.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// HTTP GET endpoint that returns all products.
        /// </summary>
        /// <returns>A collection of ProductModel objects.</returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get() => ProductService.GetProducts();

        /// <summary>
        /// HTTP PATCH endpoint that allows adding a rating to a product.
        /// </summary>
        /// <param name="request">The rating request containing product ID and rating value.</param>
        /// <returns>BadRequest if data is invalid; Ok if successful.</returns>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            // Validate request data
            if (request?.ProductId == null)
                return BadRequest(); // Return 400 if ProductId is missing or null

            // Add the rating using the service
            ProductService.AddRating(request.ProductId, request.Rating);

            return Ok(); // Return 200 on success
        }

        /// <summary>
        /// Data structure representing a rating submission.
        /// </summary>
        public class RatingRequest
        {
            /// <summary>
            /// The ID of the product to rate.
            /// </summary>
            public string? ProductId { get; set; }

            /// <summary>
            /// The rating value to add (e.g., 1–5).
            /// </summary>
            public int Rating { get; set; }
        }
    }
}

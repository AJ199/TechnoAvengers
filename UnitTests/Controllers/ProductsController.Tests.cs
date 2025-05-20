using NUnit.Framework;
using System.Linq;
using ContosoCrafts.WebSite.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for ProductsController class
    /// </summary>
    public class ProductsControllerTest
    {
        #region TestSetup
        /// <summary>
        /// Test Setup
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion

        #region GetProducts

        /// <summary>
        /// Creates a default datapoint of ProductServices
        /// Creates a new datapoint of the a ProductsController with datapoint
        /// Gets the whole datapoint
        /// Tests if Equal
        /// </summary>
        [Test]
        public void GetProducts_Data_Present_Should_Return_True()
        {
            //arrange
            //Create new default ProductService datapoint

            //Act
            //store datapoint as a ProductController datapoint
            var newData = new ProductsController(TestHelper.ProductService).Get().First();

            var response = TestHelper.ProductService.GetProducts().First();

            //Assert
            Assert.AreEqual(newData.Id, response.Id);
        }

        #endregion GetProducts

        #region Ratings

        /// <summary>
        /// Creates a default datapoint of ProductServices
        /// Creates a new datapoint of the a ProductsController with datapoint
        /// Gets the whole datapoint
        /// Tests if Added dataPoint equals the created one
        /// </summary>
        [Test]
        public void Patch_Add_Valid_Rating_Should_Return_True()
        {
            //arrange
            //Create new default ProductService datapoint

            //Act
            //store datapoint as a ProductController datapoint
            var newData = new ProductsController(TestHelper.ProductService);
            //Create a newRating datapoint to "Patch to theDataController"
            var newRating = new ProductsController.RatingRequest();
            {
                newRating.ProductId = newData.ProductService.GetProducts().Last().Id;
                newRating.Rating = 5;
            }

            //Act
            newData.Patch(newRating);

            //Assert
            Assert.AreEqual(newData.ProductService.GetProducts().Last().Id, newRating.ProductId);
        }

        /// <summary>
        /// Verifies that Patch returns BadRequest (400) when the RatingRequest is null or has a null ProductId.
        /// </summary>
        [Test]
        public void Patch_Null_Or_Missing_Product_Id_Returns_Bad_Request()
        {
            // Arrange
            var controller = new ProductsController(TestHelper.ProductService);

            // Act & Assert
            // Case 1: request is null
            var resultNull = controller.Patch(null);
            Assert.That(resultNull, Is.TypeOf<BadRequestResult>());

            // Case 2: request with null ProductId
            var requestMissingId = new ProductsController.RatingRequest
            {
                ProductId = null,
                Rating = 3
            };
            var resultMissingId = controller.Patch(requestMissingId);
            Assert.That(resultMissingId, Is.TypeOf<BadRequestResult>());
        }

        #endregion Ratings
    }
}
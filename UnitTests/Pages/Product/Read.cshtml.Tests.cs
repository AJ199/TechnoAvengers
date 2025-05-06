using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for the Read page
    /// </summary>
    public class ReadTests
    {

        #region TestSetup

        // The Read page model for testing
        public static ReadModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new ReadModel(TestHelper.ProductService);
        }
        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Validates that OnGet with valid ID returns product and no client response
        /// </summary>
        [Test]
        public void OnGet_ValidId_Should_Return_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            pageModel.OnGet(data.Id);
            var result = pageModel.Product;

            // Assert
            Assert.AreEqual(data.Title, pageModel.Product.Title);
            Assert.AreEqual(null, pageModel.ClientResponse);
        }

        /// <summary>
        /// Validates OnGet populates all product fields 
        /// </summary>
        [Test]
        public void OnGet_ValidId_Should_Populate_AllProductFields()
        {
            // Arrange
            // Find a fully populated entry
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            pageModel.OnGet(data.Id);
            var result = pageModel.Product;

            // Assert
            Assert.AreEqual(data.Title, result.Title);
            Assert.AreEqual(false, string.IsNullOrEmpty(result.ImageUrl));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.Fullname));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.Birthplace));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.Work));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.FirstAppear));
            Assert.AreEqual(true, result.Intelligence > 0);
            Assert.AreEqual(true, result.Strength > 0);
            Assert.AreEqual(true, result.Speed > 0);
            Assert.AreEqual(true, result.Durability > 0);
            Assert.AreEqual(true, result.Power > 0);
            Assert.AreEqual(true, result.Combat > 0);

        }

        /// <summary>
        /// Validates that OnGet with invalid ID should return null product
        /// </summary>
        [Test]
        public void OnGet_InvalidId_Should_Return_Null()
        {
            // Arrange

            // Act
            pageModel.OnGet("invalid-id");
            var result = pageModel.Product;

            // Assert
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Validates that OnGet with invalid ID returns null product and redirects to error
        /// </summary>
        [Test]
        public void OnGet_InvalidId_Should_Redirect_To_ErrorPage()
        {
            // Arrange

            // Act
            pageModel.OnGet("invalid-id");
            var result = (RedirectToPageResult)pageModel.ClientResponse;

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(null, pageModel.Product);
            Assert.AreEqual(false, result == null);
            Assert.AreEqual("/Error", result.PageName);
        }


        /// <summary>
        /// Validates that OnGet with null ID returns null product
        /// </summary>
        [Test]
        public void OnGet_NullId_Should_Return_Null()
        {
            // Arrange

            // Act
            pageModel.OnGet(null);

            // Assert
            Assert.AreEqual(null, pageModel.Product);

            // Reset
            pageModel.ModelState.Clear();
        }

        #endregion OnGet
    }
}
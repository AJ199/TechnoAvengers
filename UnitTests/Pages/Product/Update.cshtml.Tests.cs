using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the Update page
    /// </summary>
    public class UpdateTests
    {

        #region TestSetup

        // Update page model for testing
        public static UpdateModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new UpdateModel(TestHelper.ProductService);
        }
        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Verifies OnGet correctly loads product and 
        /// sets ModelState to valid when a valid ID is provided
        /// </summary>
        [Test]
        public void OnGet_ValidId_Should_Return_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            pageModel.OnGet(data.Id);

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(data.Title, pageModel.Product.Title);

            // Reset
            pageModel.ModelState.Clear();

        }

        /// <summary>
        /// Verifies OnGet adds a model error and 
        /// sets ModelState to invalid when invalid ID is provided
        /// </summary>
        [Test]
        public void OnGet_InvalidId_Should_Set_InvalidState()
        {
            // Arrange

            // Act
            pageModel.OnGet("invalid-id");

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);

            // Reset
            pageModel.ModelState.Clear();
        }


        #endregion OnGet

        #region OnPost
        /// <summary>
        /// Verifies that when the product is valid, OnPost updates the product and 
        /// redirects to the Index page.
        /// </summary>
        [Test]
        public void OnPost_ValidProduct_Should_UpdateAndRedirectToIndex()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.OnGet(data.Id);
            var originalName = pageModel.Product.Fullname;
            var updatedName = originalName + "_Updated";
            pageModel.Product.Fullname = updatedName;

            // Act
            var result = pageModel.OnPost();
            var redirectResult = (RedirectToPageResult)result;

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("/Product/Index", redirectResult.PageName);
            Assert.AreEqual(updatedName, pageModel.Product.Fullname);

            // Reset 
            pageModel.Product.Fullname = originalName;
            pageModel.OnPost();
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Verifies that OnPost returns the Page when ModelState is invalid.
        /// </summary>
        [Test]
        public void OnPost_Invalid_ModelState_Should_Return_PageResult()
        {
            // Arrange

            // Force an invalid error state
            pageModel.ModelState.AddModelError("bogus", "bogus error");

            // Act
            var result = (ActionResult)pageModel.OnPost();

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);
            Assert.AreEqual(typeof(PageResult), result.GetType());

            // Reset
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Verifies that OnPost redirects to the error page when the Product is null.
        /// </summary>
        [Test]
        public void OnPost_NullProduct_Should_RedirectToErrorPage()
        {
            // Arrange
            pageModel.Product = null;

            // Act
            var result = pageModel.OnPost();
            var redirectResult = (RedirectToPageResult)result;

            // Assert
            Assert.AreEqual("/Error", redirectResult.PageName);

        }

        /// <summary>
        /// Verifies that OnPost fails to update when the product ID is invalid and sets a model error.
        /// </summary>
        [Test]
        public void OnPost_InvalidId_Should_Set_ModelError()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.OnGet(data.Id);
            var originalName = pageModel.Product.Fullname;
            pageModel.Product.Id = "invalid-id";
            pageModel.Product.Fullname = "InvalidAttempt";

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);
            Assert.AreEqual(true, pageModel.ModelState.ContainsKey("UpdateFailure"));
            Assert.AreEqual(typeof(PageResult), result.GetType());

            // Reset
            pageModel.OnGet(data.Id);
            pageModel.Product.Fullname = originalName;
            pageModel.OnPost();
            pageModel.ModelState.Clear();
        }

        #endregion

    }
}
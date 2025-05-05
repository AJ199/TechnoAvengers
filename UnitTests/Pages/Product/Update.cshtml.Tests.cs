using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the Update page
    /// </summary>
    public class UpdateTests
    {

        #region TestSetup
        // Declare the model of the Update page to be used in unit tests
        public static UpdateModel pageModel;

        [SetUp]
        /// <summary>
        /// Initializes mock Update page model for testing.
        /// </summary>
        public void TestInitialize()
        {
            pageModel = new UpdateModel(TestHelper.ProductService);
        }
        #endregion TestSetup

        #region OnGet
        [Test]
        /// <summary>
        /// Verifies OnGet correctly loads product and 
        /// sets ModelState to valid when a valid ID is provided
        /// </summary>
        public void OnGet_ValidId_Should_Return_Product()
        {
            // Arrange
            var id = "1";

            // Act
            pageModel.OnGet(id);

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("Spider-Man", pageModel.Product.Title);

            // Reset
            pageModel.ModelState.Clear();

        }

        [Test]
        /// <summary>
        /// Verifies OnGet adds a model error and 
        /// sets ModelState to invalid when invalid ID is provided
        /// </summary>
        public void OnGet_InvalidId_Should_Set_InvalidState()
        {
            // Arrange
            var id = "invalid-id";

            // Act
            pageModel.OnGet(id);  // Should not find

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);

            // Reset
            pageModel.ModelState.Clear();
        }


        #endregion OnGet

        #region OnPost
        [Test]
        /// <summary>
        /// Verifies that when the product is valid, OnPost updates the product and 
        /// redirects to the Index page.
        /// </summary>
        public void OnPost_ValidProduct_Should_UpdateAndRedirectToIndex()
        {
            // Arrange
            var id = "1"; // ID for Spider-Man
            pageModel.OnGet(id);
            var originalName = pageModel.Product.Fullname;
            pageModel.Product.Fullname = "Tom Holland";

            // Act
            var result = pageModel.OnPost();
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirectResult = (RedirectToPageResult)result;

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("/Product/Index", redirectResult.PageName);
            Assert.AreEqual("Tom Holland", pageModel.Product.Fullname);

            // Reset 
            pageModel.Product.Fullname = originalName;
            pageModel.OnPost();
            pageModel.ModelState.Clear();

        }

        [Test]
        /// <summary>
        /// Verifies that OnPost returns the Page when ModelState is invalid.
        /// </summary>
        public void OnPost_Invalid_ModelState_Should_Return_PageResult()
        {
            // Arrange

            // Force an invalid error state
            pageModel.ModelState.AddModelError("bogus", "bogus error");

            // Act
            var result = pageModel.OnPost() as ActionResult;
            var stateIsValid = pageModel.ModelState.IsValid;

            // Assert
            Assert.AreEqual(false, stateIsValid);
            Assert.IsInstanceOf<PageResult>(result);

            // Reset
            pageModel.ModelState.Clear();
        }

        [Test]
        /// <summary>
        /// Verifies that OnPost redirects to the error page when the Product is null.
        /// </summary>
        public void OnPost_NullProduct_Should_RedirectToErrorPage()
        {
            // Arrange
            pageModel.Product = null;

            // Act
            var result = pageModel.OnPost();
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirectResult = (RedirectToPageResult)result;

            // Assert
            Assert.AreEqual("/Error", redirectResult.PageName);

            // Reset
        }


        [Test]
        /// <summary>
        /// Verifies that OnPost fails to update when the product ID is invalid and sets a model error.
        /// </summary>
        public void OnPost_InvalidId_Should_Set_ModelError()
        {
            // Arrange
            var id = "332";
            pageModel.OnGet(id);
            var originalName = pageModel.Product.Fullname;
            pageModel.Product.Id = "invalid-id";
            pageModel.Product.Fullname = "InvalidAttempt";

            // Act
            var result = pageModel.OnPost() as PageResult;

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);
            Assert.IsTrue(pageModel.ModelState.ContainsKey("UpdateFailure"));
            Assert.IsInstanceOf<PageResult>(result);

            // Reset
            pageModel.OnGet(id);
            pageModel.Product.Fullname = originalName;
            pageModel.OnPost();
            pageModel.ModelState.Clear();

        }

        #endregion

    }
}
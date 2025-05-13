using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the Delete page
    /// </summary>
    public class DeleteTests
    {
        #region TestSetup

        // Delete page model for testing
        public static DeleteModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new DeleteModel(TestHelper.ProductService)
            {
                // Set explicitly because of use of TempData in OnPost
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData

            };
        }

        #endregion TestSetup

        #region OnGet Tests
        /// <summary>
        /// Verifies that OnGet redirects to Index page when
        /// provided an invalid ID
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_Should_Redirect_To_Index()
        {
            // Arrange
            pageModel.Id = "invalid-id-xyz";

            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.AreEqual(typeof(RedirectToPageResult), result.GetType());
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("/Product/Index", redirect.PageName);
        }

        /// <summary>
        /// Verifies that the product is not null before proceeding with operations
        /// related to the product, ensuring valid product data is available.
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Return_Page()
        {
            // Arrange
            var validProduct = TestHelper.ProductService.GetProducts().First();
            pageModel.Id = validProduct.Id;

            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(validProduct.Id, pageModel.Product.Id); // Optional: validate the loaded product
        }

        #endregion OnGet Tests

        #region OnPost Tests
        /// <summary>
        /// Verifies that OnPost redirects to Index page when
        /// provided a valid ID
        /// </summary>
        [Test]
        public void OnPost_Valid_Id_Should_Delete_And_Redirect()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Id = data.Id;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(RedirectToPageResult), result.GetType());
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("/Product/Index", redirect.PageName);
        }

        /// <summary>
        /// Verifies that OnPost redirects to Index page when
        /// the provided ID does not exist (i.e., product is null)
        /// </summary>
        [Test]
        public void OnPost_Invalid_Id_Should_Redirect_To_Index()
        {
            // Arrange
            pageModel.Id = "non-existent-id";

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(RedirectToPageResult), result.GetType());
            var redirect = (RedirectToPageResult)result;
            Assert.AreEqual("/Product/Index", redirect.PageName);
        }

        #endregion OnPost Tests
    }
}

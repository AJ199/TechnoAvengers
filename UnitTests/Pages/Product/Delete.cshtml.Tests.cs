using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
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
        public void OnGet_InvalidId_Should_Redirect_To_Index()
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


        #endregion OnGet Tests

        #region OnPost Tests
        /// <summary>
        /// Verifies that OnPost redirects to Index page when
        /// provided a valid ID
        /// </summary>
        [Test]
        public void OnPost_ValidId_Should_Delete_And_Redirect()
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

        #endregion OnPost Tests
    }
}

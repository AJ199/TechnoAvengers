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

    }
}
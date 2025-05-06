using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit testing for Index page
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        // Index page model for testing
        public static IndexModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new IndexModel();
        }

        #endregion TestSetup
        /// <summary>
        /// Validates OnGet method loads superhero data
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange

            // Start at current test output directory
            var currentDir = Directory.GetCurrentDirectory();

            // Go up to the solution root and build relative path to products.json
            var projectRoot = Directory.GetParent(currentDir).Parent.Parent.Parent.FullName;
            var sourceFilePath = Path.Combine(projectRoot, "src", "wwwroot", "data", "products.json");
            var targetDirectory = Path.Combine(currentDir, "wwwroot", "data");
            var targetFilePath = Path.Combine(targetDirectory, "products.json");
            Directory.CreateDirectory(targetDirectory);

            var data = TestHelper.ProductService.GetProducts();
            var expectedProduct = data.First();

            // Act
            pageModel.OnGet();
            var result = pageModel.Products;

            // Assert
            Assert.AreEqual(false, result == null, "Products list should not be null");
            Assert.AreEqual(false, result.Count == 0, "Products list should not be empty");

            var resultProduct = pageModel.Products.First(p => p.Id == expectedProduct.Id);

            Assert.AreEqual(false, resultProduct == null, "Product should exist in the result list");
            Assert.AreEqual(expectedProduct.Title, resultProduct.Title, "Product titles should match");
        }
        #endregion OnGet
    }
}
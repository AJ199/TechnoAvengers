using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit testing for Index Tests
    /// </summary>
    public class IndexTests
    {
        // Database MiddleTier
        #region TestSetup
        public static IndexModel pageModel;
        /// <summary>
        /// Initialize of Test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new IndexModel();
        }

        #endregion TestSetup
        /// <summary>
        /// Checking whether product user want is there in result or not.
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            var expectedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");

            // Confirm the file exists
            Assert.IsTrue(File.Exists(expectedFilePath), $"JSON file not found at {expectedFilePath}");

            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Products, "Products list should not be null");
            Assert.IsNotEmpty(pageModel.Products, "Products list should not be empty");

            // Optionally check a known product
            var knownProduct = pageModel.Products.FirstOrDefault(p => p.Id == "620");
            Assert.IsNotNull(knownProduct, "Known product with ID 31 should exist");
            Assert.AreEqual("Spider-Man", knownProduct.Title);
        }
        #endregion OnGet
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Newtonsoft.Json;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for CreateModel Razor Page
    /// </summary>
    public class CreateTests
    {
        // Page model under test
        private CreateModel pageModel;

        /// <summary>
        /// Setup for each test: Initializes a CreateModel with mocked dependencies
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(Directory.GetCurrentDirectory());
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            pageModel = new CreateModel();
        }

        #region OnGet

        /// <summary>
        /// Test that OnGet returns a PageResult
        /// </summary>
        [Test]
        public void OnGet_Should_Return_Page()
        {
            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
        }

        /// <summary>
        /// Test that OnPost creates a new product and redirects to Read page
        /// when no products exist yet
        /// </summary>
        [Test]
        public void OnPost_ValidProduct_Should_Create_And_Redirect()
        {
            // Arrange
            var newProduct = new ProductModel
            {
                Title = "Spider-Man",
                Fullname = "Peter Parker",
                Birthplace = "Queens",
                Work = "movie"
            };

            pageModel.Product = newProduct;

            // Simulate empty JSON file
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");
            Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));
            File.WriteAllText(jsonPath, "[]");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirect = result as RedirectToPageResult;
            Assert.AreEqual("Read", redirect.PageName);
            Assert.IsNotNull(redirect.RouteValues);
            Assert.IsTrue(redirect.RouteValues.ContainsKey("id"));
        }

        /// <summary>
        /// Test that OnPost appends a new product and redirects to Read page
        /// when existing products are already present
        /// </summary>
        [Test]
        public void OnPost_ValidProduct_WithExistingProducts_ShouldAppendAndRedirect()
        {
            // Arrange
            var existingProducts = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = "1",
                    Title = "Spider-Man",
                    Fullname = "Peter Parker",
                    Birthplace = "Queens"
                }
            };

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");
            Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(existingProducts, Formatting.Indented));

            var newProduct = new ProductModel
            {
                Title = "Iron Man",
                Fullname = "Tony Stark",
                Birthplace = "New York",
                Work = "Avenger"
            };

            pageModel.Product = newProduct;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirect = result as RedirectToPageResult;
            Assert.AreEqual("Read", redirect.PageName);
            Assert.IsNotNull(redirect.RouteValues);
            Assert.IsTrue(redirect.RouteValues.ContainsKey("id"));

            // Validate the product was appended
            var finalJson = File.ReadAllText(jsonPath);
            var updatedProducts = JsonConvert.DeserializeObject<List<ProductModel>>(finalJson);

            Assert.AreEqual(2, updatedProducts.Count);
            Assert.AreEqual("Iron Man", updatedProducts.Last().Title);
        }

        #endregion OnGet
    }
}

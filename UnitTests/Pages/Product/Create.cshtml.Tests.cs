using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using ContosoCrafts.WebSite.Services;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for the Create page
    /// </summary>
    public class CreateTests
    {
        #region TestSetup
        // Create page model for testing
        private CreateModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new CreateModel(TestHelper.ProductService);
        }
        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Validates that OnGet returns a PageResult
        /// </summary>
        [Test]
        public void OnGet_Valid_Products_When_Called_Should_Return_Page()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
        }

        #endregion OnGet

        #region OnPost

        /// <summary>
        /// Validates that OnPost creates a new product and redirects to Read page
        /// </summary>
        [Test]
        public void OnPost_Valid_Product_Should_Create_And_Redirect()
        {
            // Arrange
            var data = new ProductModel
            {
                Title = "Tester hero",
                Fullname = "Tester name",
                Birthplace = "-",
                Work = "Tester",
                FirstAppear = "Movie",
                ImageUrl = "https://www.test.com/test/test.jpg",
                Intelligence = 56,
                Strength = 32,
                Speed = 35,
                Durability = 65,
                Power = 60,
                Combat = 84,
                Ratings = null
            };

            pageModel.Product = data;

            // Act
            var result = pageModel.OnPost();
            var redirectResult = (RedirectToPageResult)result;

            // Assert
            Assert.AreEqual(typeof(RedirectToPageResult), result.GetType());
            Assert.AreEqual("Read", redirectResult.PageName);
            Assert.AreEqual(false, redirectResult.RouteValues == null);
            Assert.AreEqual(true, redirectResult.RouteValues.ContainsKey("id"));
        }

        [Test]
        public void OnPost_Invalid_Model_State_Should_Return_Page()
        {
            // Arrange
            pageModel.Product = new ProductModel
            {
                Title = "", // Assuming this is a required field
            };

            pageModel.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
        }


        #endregion OnPost
    }
}
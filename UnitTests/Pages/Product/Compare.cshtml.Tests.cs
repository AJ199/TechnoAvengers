using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the Compare page
    /// </summary>
    public class CompareTests
    {
        #region TestSetup

        // Page model for Compare page
        public static CompareModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new CompareModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Verifies that OnGet populates product list and hero dropdown options
        /// </summary>
        [Test]
        public void OnGet_Should_Populate_Products_And_Hero_Options_Valid()
        {
            //Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Products);
            Assert.IsNotEmpty(pageModel.Products);
            Assert.IsNotEmpty(pageModel.HeroOptions);
        }

        #endregion OnGet Tests

        #region OnPost Tests

        /// <summary>
        /// Verifies OnPost redirects to CompareResult page when two valid different heroes are selected
        /// </summary>
        [Test]
        public void OnPost_Valid_Different_Heroes_Should_Redirect()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            pageModel.Hero1Id = products[0].Id;
            pageModel.Hero2Id = products[1].Id;

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("CompareResult", result.PageName);
        }

        /// <summary>
        /// Verifies OnPost returns current page when the same hero is selected twice
        /// </summary>
        [Test]
        public void OnPost_Valid_Same_Hero_Should_Return_Page()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = data.Id;
            pageModel.Hero2Id = data.Id;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
        }

        /// <summary>
        /// Verifies OnPost returns current page when Hero1 is missing
        /// </summary>
        [Test]
        public void OnPost_Invalid_Missing_Hero1_Should_Return_Page()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = null;
            pageModel.Hero2Id = data.Id;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
        }

        /// <summary>
        /// Verifies OnPost returns current page when Hero2 is missing
        /// </summary>
        [Test]
        public void OnPost_Invalid_Missing_Hero2_Should_Return_Page()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = data.Id;
            pageModel.Hero2Id = null;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
        }

        #endregion OnPost Tests
    }
}
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Provides unit testing for the CompareResult page
    /// </summary>
    public class CompareResultTests
    {
        #region TestSetup

        // Page model for CompareResult page
        public static CompareResultModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new CompareResultModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Verifies OnGet assigns valid Hero1 and Hero2 and returns Page result
        /// </summary>
        [Test]
        public void OnGet_Valid_Heroes_Should_Assign_And_Return_Page()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var hero1Id = products[0].Id;
            var hero2Id = products[1].Id;

            // Act
            var result = pageModel.OnGet(hero1Id, hero2Id);

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsNotNull(pageModel.Hero1);
            Assert.IsNotNull(pageModel.Hero2);
        }

        /// <summary>
        /// Verifies OnGet redirects to Compare page when Hero1 is invalid
        /// </summary>
        [Test]
        public void OnGet_Invalid_Hero1_Should_Redirect_To_Compare()
        {
            // Arrange
            var validHero = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = pageModel.OnGet("invalid-id", validHero.Id);

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("Compare", ((RedirectToPageResult)result).PageName);
        }

        /// <summary>
        /// Verifies OnGet redirects to Compare page when Hero2 is invalid
        /// </summary>
        [Test]
        public void OnGet_Invalid_Hero2_Should_Redirect_To_Compare()
        {
            // Arrange
            var validHero = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = pageModel.OnGet(validHero.Id, "invalid-id");

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("Compare", ((RedirectToPageResult)result).PageName);
        }

        /// <summary>
        /// Verifies OnGet redirects to Compare page when both heroes are invalid
        /// </summary>
        [Test]
        public void OnGet_Both_Invalid_Should_Redirect_To_Compare()
        {
            // Act
            var result = pageModel.OnGet("invalid1", "invalid2");

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("Compare", ((RedirectToPageResult)result).PageName);
        }

        #endregion OnGet Tests
    }
}

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Html;
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
        public static HtmlString DisplayStat(string label, int value, int opponent)
        {
            var isHigher = value > opponent;
            var isEqual = value == opponent;
            var css = isHigher ? "stat-row highlight-glow" : "stat-row";
            var icon = isHigher ? " 🏆" : (isEqual ? " ⚔️" : "");

            return new HtmlString($"<div class='{css}'><span>{label}</span><span>{value}{icon}</span></div>");
        }

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
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = pageModel.OnGet("invalid-id", data.Id);

            // Assert
            Assert.AreEqual(typeof(RedirectToPageResult), result.GetType());
            Assert.AreEqual("Compare", ((RedirectToPageResult)result).PageName);
        }

        /// <summary>
        /// Verifies OnGet redirects to Compare page when Hero2 is invalid
        /// </summary>
        [Test]
        public void OnGet_Invalid_Hero2_Should_Redirect_To_Compare()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = pageModel.OnGet(data.Id, "invalid-id");

            // Assert
            Assert.AreEqual(typeof(RedirectToPageResult), result.GetType());
            Assert.AreEqual("Compare", ((RedirectToPageResult)result).PageName);
        }

        /// <summary>
        /// Verifies OnGet redirects to Compare page when both heroes are invalid
        /// </summary>
        [Test]
        public void OnGet_Both_Invalid_Should_Redirect_To_Compare()
        {
            //Arrange

            // Act
            var result = pageModel.OnGet("invalid1", "invalid2");

            // Assert
            Assert.AreEqual(typeof(RedirectToPageResult), result.GetType());
            Assert.AreEqual("Compare", ((RedirectToPageResult)result).PageName);
        }

        #endregion OnGet Tests

        #region DisplayStat

        /// <summary>
        /// Verifies that DisplayStat highlights the hero when their stat is higher.
        /// </summary>
        [Test]
        public void DisplayStat_Valid_HigherStat_ReturnsHighlightedHtml()
        {
            // Arrange

            // Act
            var result = DisplayStat("Power", 90, 80) as HtmlString;

            // Assert
            Assert.IsTrue(result.Value.Contains("highlight-glow"));
            Assert.IsTrue(result.Value.Contains("🏆"));
        }

        /// <summary>
        /// Verifies that DisplayStat shows ⚔️ icon when stats are equal.
        /// </summary>
        [Test]
        public void DisplayStat_Valid_EqualStat_ReturnsEqualHtml()
        {
            // Arrange

            // Act
            var result = DisplayStat("Speed", 85, 85) as HtmlString;

            // Assert
            Assert.IsTrue(result.Value.Contains("stat-row"));
            Assert.IsTrue(result.Value.Contains("⚔️"));
        }

        /// <summary>
        /// Verifies that DisplayStat shows plain stat row when the value is lower.
        /// </summary>
        [Test]
        public void DisplayStat_Valid_LowerStat_ReturnsPlainHtml()
        {
            // Arrange

            // Act
            var result = DisplayStat("Strength", 60, 80) as HtmlString;

            // Assert
            Assert.IsTrue(result.Value.Contains("stat-row"));
            Assert.IsFalse(result.Value.Contains("highlight-glow"));
            Assert.IsFalse(result.Value.Contains("🏆"));
        }

        #endregion DisplayStat

    }
}
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for the Index page model.
    /// </summary>
    public class IndexTests
    {
        #region TestSetup

        /// <summary>
        /// The Index page model instance for testing.
        /// </summary>
        public static IndexModel pageModel;

        /// <summary>
        /// Initializes the test environment by creating a new IndexModel instance.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new IndexModel();
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Ensures OnGet loads products successfully when no sorting parameters are provided.
        /// </summary>
        [Test]
        public void OnGet_Should_Load_Products_When_No_Sort()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Products, "Products list should not be null.");
            Assert.IsNotEmpty(pageModel.Products, "Products list should not be empty.");
        }

        /// <summary>
        /// Ensures OnGet sorts products by Title in ascending order when requested.
        /// </summary>
        [Test]
        public void OnGet_Sort_By_Title_Ascending_Should_Order_Correctly()
        {
            // Arrange
            pageModel.SortField = "Title";
            pageModel.SortOrder = "asc";

            // Act
            pageModel.OnGet();

            // Assert: Titles should be sorted alphabetically in ascending order.
            var titles = pageModel.Products.Select(p => p.Title).ToList();
            var sorted = titles.OrderBy(t => t).ToList();
            Assert.IsTrue(titles.SequenceEqual(sorted));
        }

        /// <summary>
        /// Ensures OnGet sorts products by Title in descending order when requested.
        /// </summary>
        [Test]
        public void OnGet_Sort_By_Title_Descending_Should_Order_Correctly()
        {
            // Arrange
            pageModel.SortField = "Title";
            pageModel.SortOrder = "desc";

            // Act
            pageModel.OnGet();

            // Assert: Titles should be sorted alphabetically in descending order.
            var titles = pageModel.Products.Select(p => p.Title).ToList();
            var sorted = titles.OrderByDescending(t => t).ToList();
            Assert.IsTrue(titles.SequenceEqual(sorted));
        }

        /// <summary>
        /// Ensures OnGet handles an invalid SortField gracefully without applying sorting.
        /// </summary>
        [Test]
        public void OnGet_Invalid_SortField_Should_Not_Sort()
        {
            // Arrange
            pageModel.SortField = "InvalidField";

            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Products, "Products list should not be null.");
            Assert.IsNotEmpty(pageModel.Products, "Products list should not be empty.");
        }

        /// <summary>
        /// Ensures OnGet applies default ascending sorting when SortOrder is null.
        /// </summary>
        [Test]
        public void OnGet_Null_SortOrder_Should_Default_To_Ascending()
        {
            // Arrange
            pageModel.SortField = "Title";
            pageModel.SortOrder = null;

            // Act
            pageModel.OnGet();

            // Assert: Titles should be sorted alphabetically in ascending order.
            var titles = pageModel.Products.Select(p => p.Title).ToList();
            var sorted = titles.OrderBy(t => t).ToList();
            Assert.IsTrue(titles.SequenceEqual(sorted));
        }

        #endregion OnGet Tests
    }
}

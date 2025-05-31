using NUnit.Framework;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for RankModel page
    /// </summary>
    public class RankTests
    {
        #region TestSetup

        // Rank page model for testing
        public static RankModel pageModel;

        /// <summary>
        /// Initialize the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new RankModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Verify OnGet() retrieves and sorts superheroes by Score descending
        /// </summary>
        [Test]
        public void OnGet_Should_Return_Sorted_Superheroes()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Superheroes);
            Assert.IsTrue(pageModel.Superheroes.Count() > 0);

            // Check that superheroes are sorted by Score descending
            var sorted = pageModel.Superheroes.ToList();
            for (int i = 0; i < sorted.Count - 1; i++)
            {
                Assert.GreaterOrEqual(sorted[i].Score, sorted[i + 1].Score);
            }
        }

        #endregion
    }
}


using System.Linq;
using ContosoCrafts.WebSite.Models;
using NUnit.Framework;


namespace UnitTests.Services.TestJsonFileProductService
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        /// <summary>
        /// REST Get Products data
        /// POST a valid rating
        /// Test that the last data that was added was added correctly
        /// </summary>
        [Test]
        public void AddRating_ValidProductId_NoPriorRatings_ShouldInitializeRatings()
        {
            // Arrange
            var product = TestHelper.ProductService.GetProducts().FirstOrDefault(p => p.Ratings == null);

            // If all products already have Ratings, fail test setup
            Assert.IsNotNull(product, "No product found with null Ratings.");

            // Act
            TestHelper.ProductService.AddRating(product.Id, 4);

            // Assert
            var updated = TestHelper.ProductService.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            Assert.IsNotNull(updated.Ratings);
            Assert.AreEqual(1, updated.Ratings.Length);
            Assert.AreEqual(4, updated.Ratings[0]);
        }

    }
    #endregion AddRating
}
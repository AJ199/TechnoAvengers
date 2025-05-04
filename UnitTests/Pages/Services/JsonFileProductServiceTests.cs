
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
        public void AddRating_Valid_ProductId_Return_true()
        {
            // Arrange

            // Create Dummy Record with no prior Ratings
            // Get the Last data item
            var data = TestHelper.ProductService.GetProducts().Last();

            // Act
            // Store the result of the AddRating method (which is being tested)
            bool validAdd = TestHelper.ProductService.AddRating(data.Id, 0);

            // Assert
            Assert.AreEqual(true, validAdd);

            // Reset
            // Delete Dummy Record
        }

        /// <summary>
        /// REST POST data that doesn't fit the constraints defined in function
        /// Test if it Adds
        /// Returns False because it wont add
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_ID_Not_Present_Should_Return_False()
        {
            // Arrange

            // Act
            // Store the result of the AddRating method (which is being tested)
            var result = TestHelper.ProductService.AddRating("1000", 5);

            // Assert
            Assert.AreEqual(false, result);
        }

    }
    #endregion AddRating
}
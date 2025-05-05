using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using NUnit.Framework;

namespace UnitTests.Services.TestJsonFileProductService
{
    /// <summary>
    /// Unit tests for JsonFileProductService
    /// </summary>
    public class JsonFileProductServiceTests
    {
        // Service instance to access product operations
        private JsonFileProductService _productService;

        #region TestSetup

        /// <summary>
        /// Initializes the service before each test using shared test helper
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _productService = TestHelper.ProductService;
        }

        #endregion TestSetup

        #region AddRating

        /// <summary>
        /// Valid product ID with no prior ratings should create a new ratings array with one entry
        /// </summary>
        [Test]
        public void AddRating_ValidProductId_NoPriorRatings_ShouldInitializeRatings()
        {
            // Arrange
            var data = _productService.GetProducts().FirstOrDefault(p => p.Ratings == null);
            Assert.IsNotNull(data, "Product found with null Ratings.");

            // Act
            _productService.AddRating(data.Id, 4);

            // Assert
            var result = _productService.GetProducts().FirstOrDefault(p => p.Id == data.Id);
            Assert.IsNotNull(result.Ratings);
            Assert.AreEqual(1, result.Ratings.Length);
            Assert.AreEqual(4, result.Ratings[0]);
        }

        /// <summary>
        /// Valid product ID with existing ratings should append the new rating to the array
        /// </summary>
        [Test]
        public void AddRating_ValidProductId_WithPriorRatings_ShouldAppendRating()
        {
            // Arrange
            var data = _productService.GetProducts().FirstOrDefault(p => p.Ratings != null);
            Assert.IsNotNull(data, "Product found with existing Ratings.");
            var originalCount = data.Ratings.Length;

            // Act
            _productService.AddRating(data.Id, 5);

            // Assert
            var result = _productService.GetProducts().FirstOrDefault(p => p.Id == data.Id);
            Assert.AreEqual(originalCount + 1, result.Ratings.Length);
            Assert.AreEqual(5, result.Ratings.Last());
        }

        /// <summary>
        /// Invalid product ID should not throw an exception when rating is added
        /// </summary>
        [Test]
        public void AddRating_InvalidProductId_ShouldNotThrow()
        {
            // Act & Assert
            _productService.AddRating("invalid_id_123", 3);
            Assert.Pass("No exception thrown");
        }

        #endregion AddRating

        #region UpdateData

        /// <summary>
        /// Valid product should update successfully and return true
        /// </summary>
        [Test]
        public void UpdateData_ValidProduct_ShouldUpdateAndReturnTrue()
        {
            // Arrange
            var data = _productService.GetProducts().First();

            var updatedProduct = new ProductModel
            {
                Id = data.Id,
                Title = "Updated Title",
                Fullname = "Updated Name",
                Birthplace = "Updated Place",
                Work = "Updated Work",
                FirstAppear = "Updated Date",
                ImageUrl = "updated.jpg",
                Intelligence = 10,
                Strength = 10,
                Speed = 10,
                Durability = 10,
                Power = 10,
                Combat = 10,
                Ratings = data.Ratings
            };

            // Act
            var result = _productService.UpdateData(updatedProduct);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Product with nonexistent ID should return false when attempting update
        /// </summary>
        [Test]
        public void UpdateData_InvalidProduct_ShouldReturnFalse()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "nonexistent-id-123",
                Title = "Does not matter"
            };

            // Act
            var result = _productService.UpdateData(data);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Empty optional fields should default to dash ("-") after update
        /// </summary>
        [Test]
        public void UpdateData_EmptyOptionalFields_UsesDashAsDefault()
        {
            // Arrange
            var data = _productService.GetProducts().First();

            var dataToUpdate = new ProductModel
            {
                Id = data.Id,
                Title = "Update Test",
                ImageUrl = "updated.png",
                Intelligence = 10,
                Strength = 10,
                Speed = 10,
                Durability = 10,
                Power = 10,
                Combat = 10,
                Fullname = "",
                Birthplace = null,
                Work = "",
                FirstAppear = "  ",
                Ratings = new int[] { 5 }
            };

            // Act
            var result = _productService.UpdateData(dataToUpdate);

            // Assert
            Assert.AreEqual(true, result);

            var updated = TestHelper.ProductService.GetProducts().First(p => p.Id == data.Id);
            Assert.AreEqual("-", updated.Fullname);
            Assert.AreEqual("-", updated.Birthplace);
            Assert.AreEqual("-", updated.Work);
            Assert.AreEqual("-", updated.FirstAppear);
        }

        #endregion UpdateData
    }
}


using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using NUnit.Framework;


namespace UnitTests.Services.TestJsonFileProductService
{
    public class JsonFileProductServiceTests
    {
        /// <summary>
        /// Service instance set up at the beggining of each test
        /// </summary>
        private JsonFileProductService _productService;

        #region TestSetup
        /// <summary>
        /// Initializes the test environment before each test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _productService = TestHelper.ProductService;
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
            var data = _productService.GetProducts().FirstOrDefault(p => p.Ratings == null);
            Assert.IsNotNull(data, "Product found with null Ratings.");

            // Act
            _productService.AddRating(data.Id, 4);

            // Reset

            // Assert
            var result = _productService.GetProducts().FirstOrDefault(p => p.Id == data.Id);
            Assert.IsNotNull(result.Ratings);
            Assert.AreEqual(1, result.Ratings.Length);
            Assert.AreEqual(4, result.Ratings[0]);
        }

        [Test]
        public void AddRating_ValidProductId_WithPriorRatings_ShouldAppendRating()
        {
            // Arrange
            var data = _productService.GetProducts().FirstOrDefault(p => p.Ratings != null);
            Assert.IsNotNull(data, "Product found with existing Ratings.");
            var originalCount = data.Ratings.Length;

            // Act
            _productService.AddRating(data.Id, 5);

            // Reset

            // Assert
            var result = _productService.GetProducts().FirstOrDefault(p => p.Id == data.Id);
            Assert.AreEqual(originalCount + 1, result.Ratings.Length);
            Assert.AreEqual(5, result.Ratings.Last());
        }

        [Test]
        public void AddRating_InvalidProductId_ShouldNotThrow()
        {
            // Arrange

            // Act
            _productService.AddRating("invalid_id_123", 3);

            // Reset

            // Assert
            Assert.Pass("No exception thrown");
        }
        #endregion AddRating

        #region UpdateData
        /// <summary>
        /// REST POST data that doesn't fit the constraints defined in function
        /// Test if it Adds
        /// Returns False because it wont add
        /// </summary>
        [Test]
        public void UpdateData_ValidProduct_ShouldUpdateAndReturnTrue()
        {
            // Arrange
            var data = _productService.GetProducts().First();

            var updatedProduct = new ProductModel
            {
                Id = data.Id, // Must match to find and update
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

            // Reset

            // Assert
            Assert.IsTrue(result);
        }

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

            // Reset

            // Assert
            Assert.AreEqual(false,result);
        }

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
                FirstAppear = "  ",  // whitespace
                Ratings = new int[] { 5 }
            };

            // Act
            var result = _productService.UpdateData(dataToUpdate);

            // Assert
            Assert.AreEqual(true,result);

            var updated = TestHelper.ProductService.GetProducts().First(p => p.Id == data.Id);
            Assert.AreEqual("-", updated.Fullname);
            Assert.AreEqual("-", updated.Birthplace);
            Assert.AreEqual("-", updated.Work);
            Assert.AreEqual("-", updated.FirstAppear);
        }
        #endregion UpdateData

    }
}
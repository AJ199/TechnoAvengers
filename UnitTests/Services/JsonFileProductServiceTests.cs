using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;

namespace UnitTests.Services
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
        public void Add_Rating_Valid_ProductId_No_Prior_Ratings_Should_Initialize_Ratings()
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
        public void Add_Rating_Valid_Product_Id_With_Prior_Ratings_Should_Append_Rating()
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
        public void Add_Rating_Invalid_Product_Id_Should_Not_Throw_Exception()
        {
            // Act
            Assert.DoesNotThrow(() =>
            {
                _productService.AddRating("invalid_id_123", 3);
            });

            var x = true;
            Assert.IsTrue(x);
        }

        /// <summary>
        /// GetProducts returns null, AddRating should not throw and should do nothing
        /// </summary>
        [Test]
        public void Add_Rating_Null_Product_List_Should_Not_Throw()
        {
            // Arrange: Set up a temp directory and corrupted JSON file ("null")
            string tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string dataFolder = Path.Combine(tempRoot, "data");
            Directory.CreateDirectory(dataFolder);

            string tempJsonPath = Path.Combine(dataFolder, "products.json");
            File.WriteAllText(tempJsonPath, "null");

            // Mock IWebHostEnvironment to point to our temp folder
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(m => m.WebRootPath).Returns(tempRoot);
            mockEnv.Setup(m => m.ContentRootPath).Returns(tempRoot);

            var service = new JsonFileProductService(mockEnv.Object);

            // Act & Assert: Should not throw even though the JSON is 'null'
            Assert.DoesNotThrow(() => service.AddRating("some-id", 4));

            // Cleanup
            Directory.Delete(tempRoot, true);
        }

        #endregion AddRating

        #region UpdateData

        /// <summary>
        /// Valid product should update successfully and return true
        /// </summary>
        [Test]
        public void Update_Data_Valid_Product_Should_Update_And_Return_True()
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
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Product with nonexistent ID should return false when attempting update
        /// </summary>
        [Test]
        public void Update_Data_Invalid_Product_Should_Return_False()
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
        public void Update_Data_Empty_Optional_Fields_Uses_Dash_As_Default()
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

        #region CreateData

        /// <summary>
        /// Duplicate product ID should not be added and return false
        /// </summary>
        [Test]
        public void Create_Data_Duplicate_Product_Id_Should_Return_False()
        {
            // Arrange
            // Create data with duplicated id
            var data = _productService.GetProducts().First();

            var duplicate = new ProductModel
            {
                Id = data.Id,
                Title = "Duplicate Test",
                ImageUrl = "https://duplicate.png",
                Intelligence = 10,
                Strength = 10,
                Speed = 10,
                Durability = 10,
                Power = 10,
                Combat = 10
            };

            // Act
            var result = _productService.CreateData(duplicate);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Null input should return false 
        /// </summary>
        [Test]
        public void Create_Data_Null_Product_Should_Return_False()
        {
            // Act
            var result = _productService.CreateData(null);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Product with blank optional fields should store "-" as default
        /// </summary>
        [Test]
        public void Create_Data_Empty_Optional_Fields_Use_Dash_Default()
        {
            // Arrange
            var testId = "1000";

            // Remove if already exists
            var cleanupList = _productService.GetProducts().Where(p => p.Id != testId).ToList();
            _productService.SaveData(cleanupList);

            var data = new ProductModel
            {
                Id = testId,
                Title = "Empty Optional",
                ImageUrl = "https://blank.jpg",
                Intelligence = 5,
                Strength = 5,
                Speed = 5,
                Durability = 5,
                Power = 5,
                Combat = 5,
                Fullname = "",
                Birthplace = null,
                Work = "   ",
                FirstAppear = ""
            };

            // Act
            var result = _productService.CreateData(data);

            // Assert
            Assert.AreEqual(true, result);

            var saved = _productService.GetProducts().FirstOrDefault(p => p.Id == testId);
            Assert.AreEqual(false, saved == null);
            Assert.AreEqual("-", saved.Fullname);
            Assert.AreEqual("-", saved.Birthplace);
            Assert.AreEqual("-", saved.Work);
            Assert.AreEqual("-", saved.FirstAppear);
        }

        #endregion CreateData
    }
}
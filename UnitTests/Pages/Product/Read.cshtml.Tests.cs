using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for the Read page
    /// </summary>
    public class ReadTests
    {
        #region TestSetup

        // The Read page model for testing
        public static ReadModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new ReadModel(TestHelper.ProductService);
        }
        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Validates that OnGet with valid ID returns product and no client response
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Return_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            pageModel.OnGet(data.Id);
            var result = pageModel.Product;

            // Assert
            Assert.AreEqual(data.Title, pageModel.Product.Title);
            Assert.AreEqual(null, pageModel.ClientResponse);
        }

        /// <summary>
        /// Validates OnGet populates all product fields 
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Populate_All_Product_Fields()
        {
            // Arrange
            // Find a fully populated entry
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            pageModel.OnGet(data.Id);
            var result = pageModel.Product;

            // Assert
            Assert.AreEqual(data.Title, result.Title);
            Assert.AreEqual(false, string.IsNullOrEmpty(result.ImageUrl));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.Fullname));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.Birthplace));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.Work));
            Assert.AreEqual(false, string.IsNullOrEmpty(result.FirstAppear));
            Assert.AreEqual(true, result.Intelligence > 0);
            Assert.AreEqual(true, result.Strength > 0);
            Assert.AreEqual(true, result.Speed > 0);
            Assert.AreEqual(true, result.Durability > 0);
            Assert.AreEqual(true, result.Power > 0);
            Assert.AreEqual(true, result.Combat > 0);

        }

        /// <summary>
        /// Validates that OnGet with invalid ID should return null product
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            pageModel.OnGet("invalid-id");
            var result = pageModel.Product;

            // Assert
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Validates that OnGet with invalid ID returns null product and redirects to error
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_Should_Redirect_To_Error_Page()
        {
            // Arrange

            // Act
            pageModel.OnGet("invalid-id");
            var result = (RedirectToPageResult)pageModel.ClientResponse;

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(null, pageModel.Product);
            Assert.AreEqual(false, result == null);
            Assert.AreEqual("/Error", result.PageName);
        }


        /// <summary>
        /// Validates that OnGet with null ID returns null product
        /// </summary>
        [Test]
        public void OnGet_Null_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            pageModel.OnGet(null);

            // Assert
            Assert.AreEqual(null, pageModel.Product);

            // Reset
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Validates that given a valid, OnGet populates the properties related to ratings 
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Populates_Ratings_Properties()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            pageModel.OnGet(data.Id);
            var expectedCount = data.Ratings?.Length ?? 0;
            var expectedLabel = expectedCount == 1 ? "Vote" : "Votes";
            var expectedAvg = expectedCount > 0 ? data.Ratings!.Sum() / expectedCount : 0;

            // Assert
            Assert.AreEqual(expectedCount, pageModel.VoteCount);
            Assert.AreEqual(expectedLabel, pageModel.VoteLabel);
            Assert.AreEqual(expectedAvg, pageModel.CurrentRating);

            // Reset
            pageModel.ModelState.Clear();
        }
        #endregion OnGet

        #region OnPost
        /// <summary>
        /// Validates OnPost redirects to the same page with the id route given a rating
        /// </summary>
        [Test]
        public void OnPost_Valid_Rating_Provided_Redirects_To_Same_Page_With_Id()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Id = data.Id;
            var rating = 5;

            // Act
            var result = (RedirectToPageResult)pageModel.OnPost(rating);

            // assert
            Assert.IsNotNull(result, "Should return RedirectToPageResult");
            Assert.IsTrue(result.RouteValues.ContainsKey("id"));
            Assert.AreEqual(data.Id, result.RouteValues["id"]);
        }

        #endregion Onpost

        #region CalculateRating
        /// <summary>
        /// Validates CalculateRating with null ratings, sets VoteCount 
        /// and CurrentRating to zero and VoteLabel to "Votes"
        /// </summary>
        [Test]
        public void CalculateRating_Valid_Ratings_Null_Sets_Count_and_Rating_To_Zero()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "10003",
                Ratings = null
            };

            pageModel.Product = data;

            // Act
            pageModel.CalculateRating();

            // assert
            Assert.AreEqual(0, pageModel.VoteCount);
            Assert.AreEqual(0, pageModel.CurrentRating);
            Assert.AreEqual("Votes", pageModel.VoteLabel);
        }

        /// <summary>
        /// Validates that CalculateRating with one single rating setes VoteCount to 1,
        /// CurrentRating (average) to that value, and VoteLabel to "Vote"
        /// </summary>
        [Test]
        public void CalculateRating_Valid_Single_Rating_Sets_Count_One_And_Singular_Vote_Label()
        {
            // arrange
            var data = new ProductModel
            {
                Id = "x",
                Ratings = new[] { 4 }
            };

            pageModel.Product = data;

            // Act
            pageModel.CalculateRating();

            // Assert
            Assert.AreEqual(1, pageModel.VoteCount);
            Assert.AreEqual(4, pageModel.CurrentRating);
            Assert.AreEqual("Vote", pageModel.VoteLabel);
        }

        [Test]
        public void CalculateRating_Valid_Multiple_Ratings_Calculates_Average_And_Sets_Plural_Label()
        {
            // Arrange
            var ratingsArray = new[] { 3, 5, 2 }; // sum = 10, avg = 3
            var data = new ProductModel
            {
                Id = "1004",
                Ratings = ratingsArray
            };

            pageModel.Product = data;

            // Act
            pageModel.CalculateRating();

            // assert
            Assert.AreEqual(ratingsArray.Length, pageModel.VoteCount);
            Assert.AreEqual(ratingsArray.Sum() / ratingsArray.Length, pageModel.CurrentRating);
            Assert.AreEqual("Votes", pageModel.VoteLabel);
        }
        #endregion CalculateRating
    }
}
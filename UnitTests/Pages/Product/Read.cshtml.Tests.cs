using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var commentService = new JsonFileCommentService(TestHelper.MockWebHostEnvironment.Object);
            pageModel = new ReadModel(TestHelper.ProductService, commentService);
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
        public void OnGet_Invalid_Null_Id_Should_Return_Null()
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
            var expectedCount = data.Ratings.Length;
            var expectedLabel = "Votes";
            var expectedAvg = data.Ratings!.Sum() / expectedCount;

            // Assert
            Assert.AreEqual(expectedCount, pageModel.VoteCount);
            Assert.AreEqual(expectedLabel, pageModel.VoteLabel);
            Assert.AreEqual(expectedAvg, pageModel.CurrentRating);

            // Reset
            pageModel.ModelState.Clear();
        }
        #endregion OnGet

        #region OnPostAddRating
        /// <summary>
        /// Verifies that OnPostAddRating returns vote count and average when given a valid ID
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddRating_Valid_Id_Returns_Vote_Count_And_Average()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, new JsonFileCommentService(TestHelper.MockWebHostEnvironment.Object));
            var id = TestHelper.ProductService.GetProducts().First().Id;

            // Act
            var result = await model.OnPostAddRating(id, 4) as JsonResult;
            var json = result.Value.ToString();

            // Assert
            Assert.AreEqual(true, json.Contains("voteCount"));
            Assert.AreEqual(true, json.Contains("average"));
        }

        /// <summary>
        /// Validates that OnPostAddRating returns an error when given an invalid ID
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddRating_Invalid_Id_Returns_Error()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, new JsonFileCommentService(TestHelper.MockWebHostEnvironment.Object));

            // Act
            var result = await model.OnPostAddRating("invalid-id", 5) as JsonResult;
            var json = result.Value.ToString();

            // Assert
            Assert.AreEqual(true, json.Contains("error"));
        }

        /// <summary>
        /// Validates OnPostAddRating returs vote count and average when the ratings are null
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddRating_Valid_Known_Null_Ratings_Product_Uses_Empty_Fallback()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);

            // Act
            var result = await model.OnPostAddRating("714", 5) as JsonResult;
            var resultText = result.Value.ToString();

            // Assert
            Assert.AreEqual(false, result == null);
            Assert.AreEqual(true, resultText.Contains("voteCount"));
            Assert.AreEqual(true, resultText.Contains("average"));
        }

        /// <summary>
        /// Validates OnPostAddRating succesfully adds a rating to a product with null ratings and 
        /// returns the JSON with vote count and average
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddRating_Valid_Product_With_Null_Ratings_Adds_Rating_Returns_Vote_Count_And_Average()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);
            var data = new ProductModel
            {
                Id = "null-ratings-id",
                Title = "Null Ratings",
                Ratings = null
            };

            // Save to JSON
            var products = TestHelper.ProductService.GetProducts().ToList();
            products.RemoveAll(p => p.Id == data.Id);
            products.Add(data);

            File.WriteAllText(
                Path.Combine(TestHelper.MockWebHostEnvironment.Object.WebRootPath, "data", "products.json"),
                System.Text.Json.JsonSerializer.Serialize(products)
            );

            model.Product = data;

            // Act
            var result = await model.OnPostAddRating(data.Id, 1) as JsonResult;
            var text = result.Value.ToString();

            // Assert
            Assert.AreEqual(false, text == null);
            Assert.AreEqual(true, text.Contains("voteCount = 1"));
            Assert.AreEqual(true, text.Contains("average = 1"));
        }

        /// <summary>
        /// Validates that OnPostAddRating returns a JSON with votes and average when ratings are null
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddRating_Valid_Product_With_Null_Ratings_Returns_Votes_And_Average()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);

            // Manually insert a product with no ratings
            var data = new ProductModel
            {
                Id = "100009",
                Title = "No Ratings Hero",
                Ratings = null
            };

            var products = TestHelper.ProductService.GetProducts().ToList();
            products.Add(data);

            // Write modified products list back to file (only needed if the service reads from disk)
            File.WriteAllText(
                Path.Combine(TestHelper.MockWebHostEnvironment.Object.WebRootPath, "data", "products.json"),
                System.Text.Json.JsonSerializer.Serialize(products)
            );

            // Act
            var result = await model.OnPostAddRating("100009", 5) as JsonResult;

            // Assert
            var resultText = result.Value.ToString();
            Assert.AreEqual(false, resultText == null);
            Assert.AreEqual(true, resultText.Contains("voteCount"));
            Assert.AreEqual(true, resultText.Contains("average"));
        }
        #endregion

        #region OnPostAddComment
        /// <summary>
        /// Validates OnPostAddComment returs JSON when page model is valid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddComment_Valid_Model_Returns_Success_Json()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService)
            {
                PageContext = TestHelper.PageContext,
                MetadataProvider = TestHelper.ModelMetadataProvider
            };

            // Validation is enabled and succeeds
            TestHelper.EnableValidation(model);

            var id = TestHelper.ProductService.GetProducts().First().Id;

            // Clear the file so new comment is the only one
            var commentFile = Path.Combine(TestHelper.MockWebHostEnvironment.Object.WebRootPath, "data", "comments.json");
            File.WriteAllText(commentFile, "[]");

            // Act
            var result = await model.OnPostAddComment("CoverageUser", "CoverageTest", id) as JsonResult;

            // Assert
            var resultText = result.Value.ToString();
            Assert.AreEqual(true, resultText.Contains("success"));
        }

        /// <summary>
        /// Validates that OnPostAddComment returns json with expected comment fields 
        /// when the page model is valid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddComment_Valid_Model_Returns_Expected_Fields()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService)
            {
                PageContext = TestHelper.PageContext,
                MetadataProvider = TestHelper.ModelMetadataProvider
            };
            TestHelper.EnableValidation(model);

            var id = TestHelper.ProductService.GetProducts().First().Id;

            // Clear comments so only new comment exists
            var commentFile = Path.Combine(TestHelper.MockWebHostEnvironment.Object.WebRootPath, "data", "comments.json");
            File.WriteAllText(commentFile, "[]");

            // Act
            var result = await model.OnPostAddComment("CoverageUser", "CoverageTest", id) as JsonResult;

            var json = System.Text.Json.JsonSerializer.Serialize(result.Value);

            var parsed = System.Text.Json.JsonDocument.Parse(json);
            var comments = parsed.RootElement.GetProperty("comments");

            var found = false;
            foreach (var comment in comments.EnumerateArray())
            {
                var username = comment.GetProperty("Username").GetString();
                var message = comment.GetProperty("Message").GetString();
                var likes = comment.GetProperty("Likes").GetInt32();
                var commentId = comment.GetProperty("Id").GetString();

                Assert.AreEqual(false, username == null);
                Assert.AreEqual(false, message == null);
                Assert.AreEqual(true, likes >= 0);
                Assert.AreEqual(false, string.IsNullOrEmpty(commentId));
                found = true;
            }

            Assert.AreEqual(true, found); // At least one comment was checked
        }

        /// <summary>
        /// Validates that OnPostAddComment uses method AddComment when the model is valid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddComment_Valid_Model_Calls_Add_Comment()
        {
            // Arrange
            var mockCommentService = new Mock<JsonFileCommentService>(TestHelper.MockWebHostEnvironment.Object);
            var model = new ReadModel(TestHelper.ProductService, mockCommentService.Object)
            {
                PageContext = TestHelper.PageContext,
                MetadataProvider = TestHelper.ModelMetadataProvider
            };

            var id = TestHelper.ProductService.GetProducts().First().Id;
            var username = "TestUser";
            var message = "Test comment";

            TestHelper.EnableValidation(model);

            // Act
            var result = await model.OnPostAddComment(username, message, id) as JsonResult;

            // Assert: Check JsonResult still returns success
            var resultText = result.Value.ToString();
            Assert.AreEqual(true, resultText.Contains("success"));
        }

        /// <summary>
        /// Validates OnPostAddComment returns Json with errors when model is invalid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task OnPostAddComment_Invalid_Model_Returns_Failure_Json()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);

            // Act
            var result = await model.OnPostAddComment("", "", "1") as JsonResult;

            // Use reflection to get the 'errors' property
            var errorsProperty = result.Value.GetType().GetProperty("errors");
            var errors = errorsProperty.GetValue(result.Value) as IEnumerable<string>;

            // Assert
            var errorList = errors.ToList();
            Assert.AreEqual(false, errorList == null);
            Assert.AreEqual(2, errorList.Count);
            Assert.AreEqual(true, errorList.Contains("Alias is required"));
            Assert.AreEqual(true, errorList.Contains("Comment is required"));
        }
        #endregion

        #region OnPostUpdateLikes
        /// <summary>
        /// Validates OnPostUpdateLikes returns a success JSON 
        /// when given a valid comment ID
        /// </summary>
        [Test]
        public void OnPostUpdateLikes_Valid_Comment_Id_Returns_Success_Json()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);
            var data = new CommentModel { SuperheroId = "1", Username = "U", Message = "M" };
            TestHelper.CommentService.AddComment(data);

            // Act
            var result = model.OnPostUpdateLikes(data.Id, 1) as JsonResult;

            // Assert
            Assert.AreEqual(true, result.Value.ToString().Contains("success"));
        }

        /// <summary>
        /// Validates OnPostUpdateLikes returns a failure JSON when given an 
        /// invalid comment id
        /// </summary>
        [Test]
        public void OnPostUpdateLikes_Invalid_Comment_Id_Returns_Failure_Json()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);

            // Act
            var result = model.OnPostUpdateLikes("100008", 1) as JsonResult;

            // Assert
            Assert.AreEqual(true, result.Value.ToString().Contains("False"));
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

        /// <summary>
        /// Validates that CalculateRating with multiple ratings, return average and label
        /// </summary>
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

        #region ComputeRatingStats
        /// <summary>
        /// Validates ComputeRatingStats returns zero vote count and average when the 
        /// product model is null
        /// </summary>
        [Test]
        public void ComputeRatingStats_Invalid_Null_Product_Returns_Zero_Vote_Count()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);

            // Act
            var (voteCount, average) = model.ComputeRatingStats(null);

            // Assert
            Assert.AreEqual(0, voteCount);
            Assert.AreEqual(0, average);
        }

        /// <summary>
        /// Validates that ComputeRatingStats returns zero vote count and average when the 
        /// ratings are null
        /// </summary>
        [Test]
        public void ComputeRatingStats_Valid_Product_With_Null_Ratings_Returns_Zero()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);
            var product = new ProductModel { Ratings = null };

            // Act
            var result = model.ComputeRatingStats(product);
            var (voteCount, average) = ((int, int))result;

            // Assert
            Assert.AreEqual(0, voteCount);
            Assert.AreEqual(0, average);
        }
        #endregion
    }
}
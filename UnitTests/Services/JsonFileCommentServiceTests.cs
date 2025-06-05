using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for JsonFileCommentService
    /// </summary>
    public class JsonFileCommentServiceTests
    {
        #region Setup
        // Instance of JsonFileCommentService
        private JsonFileCommentService commentService;

        // Superhero ID used for testing
        private string testSuperheroId;

        // Backup of original JSON file contents
        private string originalJson;

        /// <summary>
        /// Initializes the service before each test with test configuration
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            commentService = TestHelper.CommentService;
            testSuperheroId = "test-hero-id";

            // Backup original file 
            originalJson = File.ReadAllText(GetTestFilePath());

            // Reset test data file with known state
            var initialData = new List<CommentModel>
            {
                new CommentModel { SuperheroId = testSuperheroId, Username = "Initial", Message = "Message", Likes = 0 }
            };

            File.WriteAllText(GetTestFilePath(), System.Text.Json.JsonSerializer.Serialize(initialData));
        }
        #endregion

        #region Teardown
        /// <summary>
        /// Restore original content
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            File.WriteAllText(GetTestFilePath(), originalJson);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Verifies that assigning values to properties retains the expected results through getters
        /// </summary>
        [Test]
        public void Properties_Valid_Assigned_Values_Getters_Return_Same_Values()
        {
            // Arrange
            var model = new ReadModel(TestHelper.ProductService, TestHelper.CommentService);
            var expectedComment = new CommentModel { SuperheroId = "hero-1", Username = "User1", Message = "Message1" };
            var expectedList = new List<CommentModel> { expectedComment };

            model.NewComment = expectedComment;
            model.Comments = expectedList;

            // Act
            var resultComment = model.NewComment;
            var resultList = model.Comments;

            // Assert
            Assert.AreEqual(expectedComment, resultComment);
            Assert.AreEqual(expectedList, resultList);
        }
        #endregion

        #region GetComments
        /// <summary>
        /// Verifies that GetComments returns existing comments for a known superhero ID
        /// </summary>
        [Test]
        public void GetComments_Valid_Known_Hero_Id_Returns_Expected_Values()
        {
            // Arrange

            // Act
            var result = commentService.GetComments(testSuperheroId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Initial", result[0].Username);
        }

        /// <summary>
        ///  Verifies that GetComments returns an empty list when the comment file is empty
        /// </summary>
        [Test]
        public void GetComments_Valid_Empty_File_Returns_Empty_List()
        {
            // Arrange
            File.WriteAllText(GetTestFilePath(), "[]");

            // Act
            var result = commentService.GetComments("nonexistent-id");

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Verifies that GetComments returns an empty list when the file content is null
        /// </summary>
        [Test]
        public void GetComments_Valid_Null_Deserialization_Returns_Empty_List()
        {
            // Arrange
            File.WriteAllText(GetTestFilePath(), "null");

            // Act
            var result = commentService.GetComments("Test-id");

            // Assert
            Assert.AreEqual(false, result == null);
            Assert.AreEqual(0, result.Count);
        }
        #endregion

        #region AddComment
        /// <summary>
        /// Verifies that AddComment adds a valid comment to the existing list
        /// </summary>
        [Test]
        public void AddComment_Valid_Comment_Appends_To_File()
        {
            // Arrange
            var data = new CommentModel { SuperheroId = testSuperheroId, Username = "NewUser", Message = "New message" };

            // Act
            commentService.AddComment(data);
            var result = commentService.GetComments(testSuperheroId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(true, result.Any(c => c.Username == "NewUser"));
        }

        /// <summary>
        /// Verifies that AddComment can append to a file that initially contains null
        /// </summary>
        [Test]
        public void AddComment_Valid_Null_Deserialization_Appends_To_New_List()
        {
            // Arrange
            File.WriteAllText(GetTestFilePath(), "null");
            var comment = new CommentModel { SuperheroId = testSuperheroId, Username = "NullCase", Message = "Recovered" };

            // Act
            commentService.AddComment(comment);
            var result = commentService.GetComments(testSuperheroId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("NullCase", result[0].Username);
        }

        #endregion

        #region HelperMethods
        /// <summary>
        /// Returns the test file path for the comments JSON file
        /// </summary>
        /// <returns>Absolute file path to comments.json</returns>
        public string GetTestFilePath()
        {
            return Path.Combine(TestHelper.MockWebHostEnvironment.Object.WebRootPath, "data", "comments.json");
        }
        #endregion
    }
}
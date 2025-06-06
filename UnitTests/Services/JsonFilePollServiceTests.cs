using System.IO;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for JsonFilePollService
    /// </summary>
    [TestFixture]
    public class JsonFilePollServiceTests
    {
        private JsonFilePollService _pollService;
        private string _tempRoot;
        private string _dataFolder;
        private string _jsonFilePath;

        #region TestSetup

        /// <summary>
        /// Sets up a temporary environment and mock hosting before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Create temporary directory and data folder
            _tempRoot = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _dataFolder = Path.Combine(_tempRoot, "wwwroot", "data");
            Directory.CreateDirectory(_dataFolder);

            // Define the JSON path to poll.json inside the data folder
            _jsonFilePath = Path.Combine(_dataFolder, "poll.json");

            // Mock the IWebHostEnvironment to return our temp directory as the root path
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(env => env.WebRootPath).Returns(Path.Combine(_tempRoot, "wwwroot"));

            // Initialize the poll service with mocked environment
            _pollService = new JsonFilePollService(mockEnv.Object);
        }

        /// <summary>
        /// Cleans up temporary directories after each test to avoid side effects
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempRoot))
                Directory.Delete(_tempRoot, true);
        }

        #endregion TestSetup

        #region GetPollResult

        /// <summary>
        /// If poll.json file is missing, GetPollResult should return default PollResultModel
        /// </summary>
        [Test]
        public void GetPollResult_File_Not_Exists_Should_Return_Default_Model_Invalid()
        {
            // Arrange

            // Act: Call the method
            var result = _pollService.GetPollResult();

            // Assert: Result should have zero votes
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.YesVotes);
            Assert.AreEqual(0, result.NoVotes);
        }

        /// <summary>
        /// If poll.json has valid data, GetPollResult should deserialize it correctly
        /// </summary>
        [Test]
        public void GetPollResult_Valid_File_Should_Return_Deserialized_Model()
        {
            // Arrange: Write a valid poll result to the file
            var expected = new PollResultModel { YesVotes = 2, NoVotes = 1 };
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(expected));

            // Act: Call the method
            var result = _pollService.GetPollResult();

            // Assert: Returned model should match written values
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.YesVotes);
            Assert.AreEqual(1, result.NoVotes);
        }

        /// <summary>
        /// If poll.json contains "null", GetPollResult should return default PollResultModel
        /// </summary>
        [Test]
        public void GetPollResult_Deserialization_Returns_Null_Should_Return_Default_Model_Invalid()
        {
            // Arrange: Write "null" to simulate corrupt file
            File.WriteAllText(_jsonFilePath, "null");

            // Act: Call the method
            var result = _pollService.GetPollResult();

            // Assert: Should fallback to default model
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.YesVotes);
            Assert.AreEqual(0, result.NoVotes);
        }

        #endregion GetPollResult

        #region SavePollResult

        /// <summary>
        /// SavePollResult should write the poll result to poll.json as formatted JSON
        /// </summary>
        [Test]
        public void SavePollResult_Should_Write_Correct_Json_To_File_Valid()
        {
            // Arrange: Create poll data to save
            var poll = new PollResultModel { YesVotes = 7, NoVotes = 3 };

            // Act: Save to file
            _pollService.SavePollResult(poll);

            // Assert: Read the file and deserialize to verify content
            var json = File.ReadAllText(_jsonFilePath);
            var result = JsonSerializer.Deserialize<PollResultModel>(json);

            Assert.AreEqual(7, result.YesVotes);
            Assert.AreEqual(3, result.NoVotes);
        }

        #endregion SavePollResult

        #region UpdatePoll

        /// <summary>
        /// UpdatePoll with "yes" should increment YesVotes by 1
        /// </summary>
        [Test]
        public void UpdatePoll_Yes_Vote_Should_Increment_Yes_Votes_Valid()
        {
            // Arrange: Initialize poll file with sample data
            var poll = new PollResultModel { YesVotes = 1, NoVotes = 1 };
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(poll));

            // Act: Call UpdatePoll with "yes"
            var result = _pollService.UpdatePoll("yes");

            // Assert: YesVotes should be incremented
            Assert.AreEqual(2, result.YesVotes);
            Assert.AreEqual(1, result.NoVotes);
        }

        /// <summary>
        /// UpdatePoll with "no" should increment NoVotes by 1
        /// </summary>
        [Test]
        public void UpdatePoll_No_Vote_Should_Increment_No_Votes_Invalid()
        {
            // Arrange: Write initial poll data
            var poll = new PollResultModel { YesVotes = 5, NoVotes = 0 };
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(poll));

            // Act: Call UpdatePoll with "no"
            var result = _pollService.UpdatePoll("no");

            // Assert: NoVotes should be incremented
            Assert.AreEqual(5, result.YesVotes);
            Assert.AreEqual(1, result.NoVotes);
        }

        /// <summary>
        /// UpdatePoll with an invalid string should not change vote counts
        /// </summary>
        [Test]
        public void UpdatePoll_Invalid_Vote_Should_Not_Change_Votes()
        {
            // Arrange: Setup poll data
            var poll = new PollResultModel { YesVotes = 4, NoVotes = 2 };
            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(poll));

            // Act: Call UpdatePoll with invalid response
            var result = _pollService.UpdatePoll("maybe");

            // Assert: Votes should remain unchanged
            Assert.AreEqual(4, result.YesVotes);
            Assert.AreEqual(2, result.NoVotes);
        }

        #endregion UpdatePoll
    }
}

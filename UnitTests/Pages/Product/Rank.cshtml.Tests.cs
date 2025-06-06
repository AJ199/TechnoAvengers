using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for the Rank page
    /// </summary>
    public class RankTests
    {
        // RankModel instance under test
        public static RankModel pageModel;

        #region TestSetup

        /// <summary>
        /// Initializes the test environment and dependencies
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            var productService = TestHelper.ProductService;
            var pollService = TestHelper.PollService;
            pageModel = new RankModel(productService, pollService);
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Verifies that OnGet loads superheroes sorted by score
        /// and initializes poll results from storage
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Load_Superheroes_And_PollData()
        {
            // Act
            pageModel.OnGet();

            // Assert
            var expected = TestHelper.ProductService.GetProducts().OrderByDescending(p => p.Score).Select(p => p.Id);
            var actual = pageModel.Superheroes.Select(p => p.Id);
            Assert.IsTrue(expected.SequenceEqual(actual));
            Assert.GreaterOrEqual(pageModel.YesVotes, 0);
            Assert.GreaterOrEqual(pageModel.NoVotes, 0);
        }

        #endregion OnGet

        #region OnPost

        /// <summary>
        /// Verifies that a "yes" response increments YesVotes and returns the same page
        /// </summary>
        [Test]
        public void OnPost_Valid_Yes_Response_Should_Increment_YesVotes()
        {
            // Arrange
            pageModel.PollResponse = "yes";
            var oldVotes = TestHelper.PollService.GetPollResult().YesVotes;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(oldVotes + 1, pageModel.YesVotes);
        }

        /// <summary>
        /// Verifies that a "no" response increments NoVotes and returns the same page
        /// </summary>
        [Test]
        public void OnPost_Invalid_No_Response_Should_Increment_NoVotes()
        {
            // Arrange
            pageModel.PollResponse = "no";
            var oldVotes = TestHelper.PollService.GetPollResult().NoVotes;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(oldVotes + 1, pageModel.NoVotes);
        }

        /// <summary>
        /// Verifies that an invalid response does not affect vote counts
        /// </summary>
        [Test]
        public void OnPost_Invalid_Response_Should_Not_Change_Votes()
        {
            // Arrange
            var old = TestHelper.PollService.GetPollResult();
            pageModel.PollResponse = "invalid";

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(old.YesVotes, pageModel.YesVotes);
            Assert.AreEqual(old.NoVotes, pageModel.NoVotes);
        }

        #endregion OnPost

        #region PercentageTests

        /// <summary>
        /// Verifies YesPercentage is calculated correctly when votes exist
        /// </summary>
        [Test]
        public void YesPercentage_Should_Return_Correct_Percentage_Valid()
        {
            typeof(RankModel).GetProperty("YesVotes").SetValue(pageModel, 3);
            typeof(RankModel).GetProperty("NoVotes").SetValue(pageModel, 1);

            var result = pageModel.YesPercentage;

            Assert.AreEqual(75, result);
        }

        /// <summary>
        /// Verifies NoPercentage is calculated correctly when votes exist
        /// </summary>
        [Test]
        public void NoPercentage_Should_Return_Correct_Percentage_Invalid()
        {
            typeof(RankModel).GetProperty("YesVotes").SetValue(pageModel, 1);
            typeof(RankModel).GetProperty("NoVotes").SetValue(pageModel, 3);

            var result = pageModel.NoPercentage;

            Assert.AreEqual(75, result);
        }

        /// <summary>
        /// Verifies HasVoted returns true when total votes are greater than zero
        /// </summary>
        [Test]
        public void HasVoted_Should_Be_True_When_Votes_Exist_Valid()
        {
            typeof(RankModel).GetProperty("YesVotes").SetValue(pageModel, 1);
            typeof(RankModel).GetProperty("NoVotes").SetValue(pageModel, 1);

            Assert.IsTrue(pageModel.HasVoted);
        }

        /// <summary>
        /// Verifies HasVoted returns false when no votes exist
        /// </summary>
        [Test]
        public void HasVoted_Should_Be_False_When_No_Votes_Invalid()
        {
            typeof(RankModel).GetProperty("YesVotes").SetValue(pageModel, 0);
            typeof(RankModel).GetProperty("NoVotes").SetValue(pageModel, 0);

            Assert.IsFalse(pageModel.HasVoted);
        }

        /// <summary>
        /// Verifies CalculatePercentage returns zero when total is zero (to prevent divide-by-zero)
        /// </summary>
        [Test]
        public void CalculatePercentage_When_Total_Zero_Should_Return_Zero_Invalid()
        {
            var method = typeof(RankModel).GetMethod("CalculatePercentage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = method.Invoke(pageModel, new object[] { 0, 0 });

            Assert.AreEqual(0, result);
        }

        #endregion PercentageTests
    }
}

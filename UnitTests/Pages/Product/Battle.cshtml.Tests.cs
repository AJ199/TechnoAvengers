using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;
using static ContosoCrafts.WebSite.Pages.Product.BattleModel;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit test suite for the Battle page.
    /// Validates hero selection, prediction, results, and battle logic.
    /// </summary>
    public class BattleTests
    {
        #region TestSetup

        /// <summary>
        /// BattleModel instance used for testing.
        /// </summary>
        public static BattleModel pageModel;

        /// <summary>
        /// Initializes a fresh BattleModel before each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Arrange: create a new instance with the shared TestHelper service
            pageModel = new BattleModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Ensures OnGet populates Products and HeroOptions.
        /// </summary>
        [Test]
        public void OnGet_Valid__Should_Populate_Products_And_Hero_Options()
        {
            // Act: load the page
            pageModel.OnGet();

            // Assert: product collection and dropdown options are set
            Assert.IsNotNull(pageModel.Products);
            Assert.IsNotEmpty(pageModel.Products);
            Assert.IsNotEmpty(pageModel.HeroOptions);
        }

        #endregion OnGet Tests

        #region OnPost Tests - SelectHeroes Step

        /// <summary>
        /// Selecting the same hero twice should trigger a validation error.
        /// </summary>
        [Test]
        public void OnPost_Invalid_Select_Heroes_Same_Hero_Should_Return_Page()
        {
            // Arrange: assign identical IDs to both slots
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = data.Id;
            pageModel.Hero2Id = data.Id;
            pageModel.Step = BattleStep.SelectHeroes;

            // Act: attempt to move past selection
            var result = pageModel.OnPost();

            // Assert: remain on page with an error
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
        }

        /// <summary>
        /// Missing Hero1 selection should stay on page with error.
        /// </summary>
        [Test]
        public void OnPost_Invalid_Select_Heroes_Missing_Hero1_Should_Return_Page()
        {
            // Arrange: omit Hero1
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = null;
            pageModel.Hero2Id = data.Id;
            pageModel.Step = BattleStep.SelectHeroes;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
        }

        /// <summary>
        /// Missing Hero2 selection should stay on page with error.
        /// </summary>
        [Test]
        public void OnPost_Invalid_Select_Heroes_Missing_Hero2_Should_Return_Page()
        {
            // Arrange: omit Hero2
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = data.Id;
            pageModel.Hero2Id = null;
            pageModel.Step = BattleStep.SelectHeroes;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
        }

        /// <summary>
        /// Valid different selections should advance to prediction step.
        /// </summary>
        [Test]
        public void OnPost_Select_Heroes_Valid_Should_Proceed_To_Vote_Winner()
        {
            // Arrange: choose two distinct heroes
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            pageModel.Hero1Id = products[0].Id;
            pageModel.Hero2Id = products[1].Id;
            pageModel.Step = BattleStep.SelectHeroes;

            // Act
            var result = pageModel.OnPost();

            // Assert: moved to VoteWinner step and heroes loaded
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(BattleStep.VoteWinner, pageModel.Step);
            Assert.AreEqual(products[0].Id, pageModel.Hero1.Id);
            Assert.AreEqual(products[1].Id, pageModel.Hero2.Id);
        }

        #endregion OnPost Tests - SelectHeroes Step

        #region OnPost Tests - VoteWinner Step

        /// <summary>
        /// Failing to predict a winner should stay on page with error and retain hero data.
        /// </summary>
        [Test]
        public void OnPost_Invalid_Vote_Winner_Missing_Predicted_Winner_Should_Return_Page_And_Populate_Heroes()
        {
            // Arrange: select heroes but no prediction
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            pageModel.Hero1Id = products[0].Id;
            pageModel.Hero2Id = products[1].Id;
            pageModel.PredictedWinnerId = null;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert: error and heroes still set
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
            Assert.IsNotNull(pageModel.Hero1);
            Assert.IsNotNull(pageModel.Hero2);
        }

        /// <summary>
        /// Correct prediction advances to result and sets success message.
        /// </summary>
        [Test]
        public void OnPost_Vote_Winner_Valid_Correct_Prediction_Should_Set_Correct_Result_Message_And_Predicted_Winner()
        {
            // Arrange: identify a stronger hero and predict them
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strong = products.OrderByDescending(p =>
                p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weak = products.First(p => p.Id != strong.Id);
            pageModel.Hero1Id = strong.Id;
            pageModel.Hero2Id = weak.Id;
            pageModel.PredictedWinnerId = strong.Id;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert: moved to ShowResult, prediction correct
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
            Assert.AreEqual("You predicted correctly! 🎉", pageModel.ResultMessage);
            Assert.AreEqual(strong.Id, pageModel.PredictedWinner.Id);
        }

        /// <summary>
        /// Incorrect prediction still shows result with failure message.
        /// </summary>
        [Test]
        public void OnPost_Vote_Winner_Invalid_Prediction_Should_Set_Wrong_Result_Message_And_Step_Show_Result()
        {
            // Arrange: predict the weaker hero
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strong = products.OrderByDescending(p =>
                p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weak = products.First(p => p.Id != strong.Id);
            pageModel.Hero1Id = strong.Id;
            pageModel.Hero2Id = weak.Id;
            pageModel.PredictedWinnerId = weak.Id;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert: ShowResult with incorrect message
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
            Assert.AreEqual("Oops! Your prediction was wrong. 😢", pageModel.ResultMessage);
            Assert.AreEqual(weak.Id, pageModel.PredictedWinner.Id);
        }

        /// <summary>
        /// Verifies actual winner and loser are set when Hero2 is stronger.
        /// </summary>
        [Test]
        public void OnPost_Vote_Winner_Hero2_Is_Stronger_Should_Set_Actual_Winner_And_Loser_Valid()
        {
            // Arrange: swap roles so second hero is stronger
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strong = products.OrderByDescending(p =>
                p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weak = products.First(p => p.Id != strong.Id);
            pageModel.Hero1Id = weak.Id;
            pageModel.Hero2Id = strong.Id;
            pageModel.PredictedWinnerId = strong.Id;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert: winner/loser identified correctly
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(strong.Id, pageModel.ActualWinner.Id);
            Assert.AreEqual(weak.Id, pageModel.Loser.Id);
        }

        /// <summary>
        /// Missing Hero1 should still progress to result but skip stat logic.
        /// </summary>
        [Test]
        public void OnPost_Vote_Winner_Hero1_Invalid_Should_Skip_Stats_And_Show_Result()
        {
            // Arrange: invalid Hero1Id
            var valid = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = "invalid-id";
            pageModel.Hero2Id = valid.Id;
            pageModel.PredictedWinnerId = valid.Id;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert: no stats calculated, but step advances
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
            Assert.IsNull(pageModel.ActualWinner);
            Assert.IsNull(pageModel.Loser);
        }

        /// <summary>
        /// Missing Hero2 should still progress to result but skip stat logic.
        /// </summary>
        [Test]
        public void OnPost_Vote_Winner_Hero2_Invalid_Should_Skip_Stats_And_Show_Result()
        {
            // Arrange: invalid Hero2Id
            var valid = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = valid.Id;
            pageModel.Hero2Id = "invalid-id";
            pageModel.PredictedWinnerId = valid.Id;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
            Assert.IsNull(pageModel.ActualWinner);
            Assert.IsNull(pageModel.Loser);
        }

        #endregion OnPost Tests - VoteWinner Step

        #region Other Scenarios

        /// <summary>
        /// If Step is already ShowResult, OnPost should simply return the page.
        /// </summary>
        [Test]
        public void OnPost_Other_Step_Should_Return_Page_Valid ()
        {
            // Arrange: set step to final
            pageModel.Step = BattleStep.ShowResult;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
        }

        /// <summary>
        /// Verifies that the Loser property is assigned after a battle completes.
        /// </summary>
        [Test]
        public void Get_Loser_Should_Return_Valid_Assigned_Loser()
        {
            // Arrange: simulate a correct prediction to set Loser
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strong = products.OrderByDescending(p =>
                p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weak = products.First(p => p.Id != strong.Id);
            pageModel.Hero1Id = strong.Id;
            pageModel.Hero2Id = weak.Id;
            pageModel.PredictedWinnerId = strong.Id;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            pageModel.OnPost();
            var loser = pageModel.Loser;

            // Assert
            Assert.IsNotNull(loser);
            Assert.AreEqual(weak.Id, loser.Id);
        }

        #endregion Other Scenarios
    }
}

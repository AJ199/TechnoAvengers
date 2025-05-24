using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit test suite for the Battle page
    /// </summary>
    public class BattleTests
    {
        #region TestSetup

        public static BattleModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new BattleModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Ensures OnGet loads products and dropdown options
        /// </summary>
        [Test]
        public void OnGet_Should_Populate_Products_And_HeroOptions()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Products);
            Assert.IsNotEmpty(pageModel.Products);
            Assert.IsNotEmpty(pageModel.HeroOptions);
        }

        #endregion OnGet Tests

        #region OnPost Tests - SelectHeroes Step

        /// <summary>
        /// Ensures OnPost returns PageResult when same hero is selected
        /// </summary>
        [Test]
        public void OnPost_SelectHeroes_Same_Hero_Should_Return_Page()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = data.Id;
            pageModel.Hero2Id = data.Id;
            pageModel.Step = BattleStep.SelectHeroes;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
        }

        /// <summary>
        /// Ensures OnPost returns PageResult when Hero1 is null
        /// </summary>
        [Test]
        public void OnPost_SelectHeroes_Missing_Hero1_Should_Return_Page()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = null;
            pageModel.Hero2Id = data.Id;
            pageModel.Step = BattleStep.SelectHeroes;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
        }

        /// <summary>
        /// Ensures OnPost proceeds to VoteWinner step with valid different heroes
        /// </summary>
        [Test]
        public void OnPost_SelectHeroes_Valid_Should_Proceed_To_VoteWinner()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            pageModel.Hero1Id = products[0].Id;
            pageModel.Hero2Id = products[1].Id;
            pageModel.Step = BattleStep.SelectHeroes;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.AreEqual(BattleStep.VoteWinner, pageModel.Step);
            Assert.AreEqual(products[0].Id, pageModel.Hero1.Id);
            Assert.AreEqual(products[1].Id, pageModel.Hero2.Id);
        }

        #endregion OnPost Tests - SelectHeroes Step

        #region OnPost Tests - VoteWinner Step

        /// <summary>
        /// Ensures OnPost returns PageResult when PredictedWinnerId is missing
        /// </summary>
        [Test]
        public void OnPost_VoteWinner_Missing_PredictedWinner_Should_Return_Page()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            pageModel.Hero1Id = products[0].Id;
            pageModel.Hero2Id = products[1].Id;
            pageModel.PredictedWinnerId = null;
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
        }

        /// <summary>
        /// Verifies that when the user's prediction matches the actual winner,
        /// the result message confirms a correct prediction
        /// </summary>
        [Test]
        public void OnPost_VoteWinner_Valid_Correct_Prediction_Should_Set_Correct_ResultMessage()
        {
            // Arrange: Predicting Hulk (stronger) wins against Groot (weaker)
            const string hulkId = "332";
            const string grootId = "303";

            pageModel.Hero1Id = hulkId;
            pageModel.Hero2Id = grootId;
            pageModel.PredictedWinnerId = hulkId; // Correct prediction
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
            Assert.AreEqual("You predicted correctly! 🎉", pageModel.ResultMessage);
        }

        /// <summary>
        /// Verifies that when the user's prediction does not match the actual winner,
        /// the result message confirms an incorrect prediction
        /// </summary>
        [Test]
        public void OnPost_VoteWinner_Invalid_Prediction_Should_Set_Wrong_ResultMessage()
        {
            // Arrange: Predicting Groot (weaker) wins against Hulk (stronger)
            const string hulkId = "332";
            const string grootId = "303";

            pageModel.Hero1Id = hulkId;
            pageModel.Hero2Id = grootId;
            pageModel.PredictedWinnerId = grootId; // Incorrect prediction
            pageModel.Step = BattleStep.VoteWinner;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
            Assert.AreEqual("Oops! Your prediction was wrong. 😢", pageModel.ResultMessage);
        }


        #endregion OnPost Tests - VoteWinner Step
    }
}
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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
        /// The Battle page model instance used for testing.
        /// </summary>
        public static BattleModel pageModel;

        /// <summary>
        /// Initializes the Battle page model before each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new BattleModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Ensures OnGet populates Products and HeroOptions.
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
        /// Validates that selecting the same hero for both slots results in an error and stays on the page.
        /// </summary>
        [Test]
        public void OnPost_SelectHeroes_Same_Hero_Should_Return_Page()
        {
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = data.Id;
            pageModel.Hero2Id = data.Id;
            pageModel.Step = BattleStep.SelectHeroes;

            var result = pageModel.OnPost();

            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
        }

        /// <summary>
        /// Ensures that missing Hero1Id triggers an error and remains on the page.
        /// </summary>
        [Test]
        public void OnPost_SelectHeroes_Missing_Hero1_Should_Return_Page()
        {
            var data = TestHelper.ProductService.GetProducts().First();
            pageModel.Hero1Id = null;
            pageModel.Hero2Id = data.Id;
            pageModel.Step = BattleStep.SelectHeroes;

            var result = pageModel.OnPost();

            Assert.AreEqual(typeof(PageResult), result.GetType());
        }

        /// <summary>
        /// Ensures that selecting two valid different heroes progresses to the VoteWinner step.
        /// </summary>
        [Test]
        public void OnPost_SelectHeroes_Valid_Should_Proceed_To_VoteWinner()
        {
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            pageModel.Hero1Id = products[0].Id;
            pageModel.Hero2Id = products[1].Id;
            pageModel.Step = BattleStep.SelectHeroes;

            var result = pageModel.OnPost();

            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.AreEqual(BattleStep.VoteWinner, pageModel.Step);
            Assert.AreEqual(products[0].Id, pageModel.Hero1.Id);
            Assert.AreEqual(products[1].Id, pageModel.Hero2.Id);
        }

        #endregion OnPost Tests - SelectHeroes Step

        #region OnPost Tests - VoteWinner Step

        /// <summary>
        /// Verifies that not selecting a predicted winner triggers an error and stays on the page.
        /// </summary>
        [Test]
        public void OnPost_VoteWinner_Missing_PredictedWinner_Should_Return_Page()
        {
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            pageModel.Hero1Id = products[0].Id;
            pageModel.Hero2Id = products[1].Id;
            pageModel.PredictedWinnerId = null;
            pageModel.Step = BattleStep.VoteWinner;

            var result = pageModel.OnPost();

            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.IsTrue(pageModel.ModelState.ErrorCount > 0);
        }

        /// <summary>
        /// Ensures correct prediction results in "You predicted correctly!" message.
        /// </summary>
        [Test]
        public void OnPost_VoteWinner_Valid_Correct_Prediction_Should_Set_Correct_ResultMessage()
        {
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strongHero = products.OrderByDescending(p => p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weakHero = products.First(p => p.Id != strongHero.Id);

            pageModel.Hero1Id = strongHero.Id;
            pageModel.Hero2Id = weakHero.Id;
            pageModel.PredictedWinnerId = strongHero.Id;
            pageModel.Step = BattleStep.VoteWinner;

            var result = pageModel.OnPost();

            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
            Assert.IsNotNull(pageModel.ActualWinner);
            Assert.AreEqual(strongHero.Id, pageModel.ActualWinner.Id);
            Assert.AreEqual("You predicted correctly! 🎉", pageModel.ResultMessage);
        }

        /// <summary>
        /// Ensures incorrect prediction results in "Oops! Your prediction was wrong." message.
        /// </summary>
        [Test]
        public void OnPost_VoteWinner_Invalid_Prediction_Should_Set_Wrong_ResultMessage()
        {
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strongHero = products.OrderByDescending(p => p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weakHero = products.First(p => p.Id != strongHero.Id);

            pageModel.Hero1Id = strongHero.Id;
            pageModel.Hero2Id = weakHero.Id;
            pageModel.PredictedWinnerId = weakHero.Id;
            pageModel.Step = BattleStep.VoteWinner;

            var result = pageModel.OnPost();

            Assert.AreEqual(typeof(PageResult), result.GetType());
            Assert.AreEqual("Oops! Your prediction was wrong. 😢", pageModel.ResultMessage);
        }

        /// <summary>
        /// Verifies Hero2 wins when stronger stats are present.
        /// </summary>
        [Test]
        public void OnPost_VoteWinner_Hero2IsStronger_Should_Set_ActualWinner_As_Hero2()
        {
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strongHero = products.OrderByDescending(p => p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weakHero = products.First(p => p.Id != strongHero.Id);

            pageModel.Hero1Id = weakHero.Id;
            pageModel.Hero2Id = strongHero.Id;
            pageModel.PredictedWinnerId = strongHero.Id;
            pageModel.Step = BattleStep.VoteWinner;

            var result = pageModel.OnPost();

            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(strongHero.Id, pageModel.ActualWinner.Id);
            Assert.AreEqual(weakHero.Id, pageModel.Loser.Id);
        }

        /// <summary>
        /// Ensures OnPost handles unexpected steps gracefully by returning PageResult.
        /// </summary>
        [Test]
        public void OnPost_OtherStep_Should_ReturnPage()
        {
            pageModel.Step = BattleStep.ShowResult;

            var result = pageModel.OnPost();

            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(BattleStep.ShowResult, pageModel.Step);
        }

        /// <summary>
        /// Verifies that Loser property is correctly set after a battle.
        /// </summary>
        [Test]
        public void Get_Loser_Should_Return_Assigned_Loser()
        {
            var products = TestHelper.ProductService.GetProducts().Take(2).ToList();
            var strongHero = products.OrderByDescending(p => p.Strength + p.Intelligence + p.Speed + p.Durability + p.Power + p.Combat).First();
            var weakHero = products.First(p => p.Id != strongHero.Id);

            pageModel.Hero1Id = strongHero.Id;
            pageModel.Hero2Id = weakHero.Id;
            pageModel.PredictedWinnerId = strongHero.Id;
            pageModel.Step = BattleStep.VoteWinner;

            pageModel.OnPost();
            var loser = pageModel.Loser;

            Assert.IsNotNull(loser);
            Assert.AreEqual(weakHero.Id, loser.Id);
        }

        #endregion OnPost Tests - VoteWinner Step
    }
}

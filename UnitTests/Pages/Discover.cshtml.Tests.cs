using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the Discover page.
    /// </summary>
    public class DiscoverTests
    {
        #region TestSetup

        // The Discover page model for testing
        public static DiscoverModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new DiscoverModel();
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Ensures the page loads correctly and title is set.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_Title()
        {
            // Arrange & Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual("Discover Your Avenger", pageModel.ViewData["Title"]);
        }

        #endregion OnGet
    }
}

using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the Discover page.
    /// </summary>
    public class DiscoverTests
    {
        #region TestSetup

        // The Discover page model being tested
        public static DiscoverModel pageModel;

        /// <summary>
        /// Initializes the test environment by creating a DiscoverModel instance
        /// and setting up the necessary PageContext, ViewData, and TempData.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create ModelState and ViewData for the page model
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Instantiate DiscoverModel and set its context
            pageModel = new DiscoverModel
            {
                PageContext = new PageContext
                {
                    ViewData = viewData
                },
                TempData = tempData
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Tests that OnGet() correctly sets the ViewData["Title"] value.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_Title()
        {
            // Act: Call the OnGet() method
            pageModel.OnGet();

            // Assert: Check that the page title is set correctly
            Assert.AreEqual("Discover Your Avenger", pageModel.ViewData["Title"]);
        }

        /// <summary>
        /// Verifies that the DiscoverModel instance is not null after setup.
        /// </summary>
        [Test]
        public void DiscoverModel_Should_Not_Be_Null()
        {
            // Assert: Confirm the page model instance is initialized
            Assert.IsNotNull(pageModel);
        }

        #endregion OnGet
    }
}

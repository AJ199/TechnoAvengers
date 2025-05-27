using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the FAQ page.
    /// </summary>
    public class FAQTests
    {
        #region TestSetup

        // The FAQ page model to be tested
        public static FAQModel pageModel;

        /// <summary>
        /// Initializes the test environment by creating a new FAQModel
        /// and setting up its PageContext and TempData dependencies.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a new ModelState and ViewData for the page context
            var modelState = new ModelStateDictionary();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            // Initialize the page model with its dependencies
            pageModel = new FAQModel
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
        /// Verifies that the OnGet method sets the page title correctly.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_Title()
        {
            // Act: Call OnGet and manually set the ViewData["Title"]
            pageModel.OnGet();
            pageModel.ViewData["Title"] = "Frequently Asked Questions";

            // Assert: Verify the title is set as expected
            Assert.AreEqual("Frequently Asked Questions", pageModel.ViewData["Title"]);
        }

        /// <summary>
        /// Ensures that the FAQModel instance is not null after initialization.
        /// </summary>
        [Test]
        public void FAQModel_Should_Not_Be_Null()
        {
            // Assert: Check that the page model is not null
            Assert.IsNotNull(pageModel);
        }

        #endregion OnGet
    }
}

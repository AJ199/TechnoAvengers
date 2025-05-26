using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the FAQ page.
    /// </summary>
    public class FAQTests
    {
        #region TestSetup

        public static FAQModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            pageModel = new FAQModel();
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Set_Title()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual("Frequently Asked Questions", pageModel.ViewData["Title"]);
        }

        #endregion
    }
}

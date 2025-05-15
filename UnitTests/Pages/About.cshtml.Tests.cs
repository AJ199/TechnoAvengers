using ContosoCrafts.WebSite.Pages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Provides unit testing for the About page
    /// </summary>
    public class AboutTests
    {
        #region TestSetup

        // Page model for About page
        public static AboutModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new AboutModel();
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Verifies that OnGet executes without errors
        /// </summary>
        [Test]
        public void OnGet_Should_Work_Without_Error()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel);
        }

        #endregion OnGet Tests
    }
}

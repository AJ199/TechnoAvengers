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
        /// Verifies that OnGet leaves ModelState valid
        /// </summary>
        [Test]
        public void OnGet_Valid_Default_Execution_Model_State_Should_Be_Valid()
        {
            // Arrange
    
            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
        }

        #endregion OnGet Tests
    }
}
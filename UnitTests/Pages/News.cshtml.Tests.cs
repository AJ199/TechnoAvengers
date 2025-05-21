using ContosoCrafts.WebSite.Pages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Provides unit testing for the News page
    /// </summary>
    public class NewsTests
    {
        #region TestSetup

        // Page model for News page
        public static NewsModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new NewsModel();
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Verifies that OnGet leaves ModelState valid
        /// </summary>
        [Test]
        public void OnGet_Valid_Default_Execution_Model_State_Should_Be_Valid()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
        }

        #endregion OnGet Tests
    }
}
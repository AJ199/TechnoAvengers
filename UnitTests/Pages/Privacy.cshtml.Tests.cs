using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Provides unit testing for the Privacy page
    /// </summary>
    public class PrivacyTests
    {
        #region TestSetup

        // Page model for Privacy page
        public static PrivacyModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Mock the ILogger<PrivacyModel>
            var mockLogger = new Mock<ILogger<PrivacyModel>>();
            pageModel = new PrivacyModel(mockLogger.Object);
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
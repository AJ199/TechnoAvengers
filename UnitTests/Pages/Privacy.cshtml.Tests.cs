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

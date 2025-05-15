using ContosoCrafts.WebSite.Pages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Provides unit testing for the Contact page
    /// </summary>
    public class ContactTests
    {
        #region TestSetup

        // Page model for Contact page
        public static ContactModel pageModel;

        /// <summary>
        /// Initializes the test environment
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new ContactModel();
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

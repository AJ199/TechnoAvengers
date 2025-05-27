using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
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

        // Email service used for testing
        public static EmailService emailService;

        /// <summary>
        /// Initializes the test environment with test email settings
        /// </summary>
        [SetUp]

        public void TestInitialize()
        {
            var settings = TestHelper.LoadEmailSettings();
            var emailService = new EmailService(settings);
            pageModel = new ContactModel(emailService);

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
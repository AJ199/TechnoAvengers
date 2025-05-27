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
        /// Validates that SentToUser and SentToAdmin are true when TempData["Success"] is true
        /// </summary>
        [Test]
        public void OnGet_Valid_TempData_Should_Set_Flags_True()
        {
            // Arrange
            var settings = TestHelper.LoadEmailSettings();
            var emailService = new EmailService(settings);
            var model = new ContactModel(emailService);

            model.TempData = TestHelper.TempData;
            model.TempData["Success"] = true;

            // Act
            model.OnGet();

            // Assert
            Assert.AreEqual(true, model.SentToUser);
            Assert.AreEqual(true, model.SentToAdmin);
            Assert.IsNotNull(model.Form);
        }

        /// <summary>
        /// Validates that form is initialized when TempData is missing
        /// </summary>
        [Test]
        public void OnGet_Invalid_TempData_Missing_Should_Only_Initialize_Form()
        {
            // Arrange
            var settings = TestHelper.LoadEmailSettings();
            var emailService = new EmailService(settings);
            var model = new ContactModel(emailService);
            model.TempData = TestHelper.TempData;

            // Act
            model.OnGet();

            // Assert
            Assert.AreEqual(false, model.SentToUser);
            Assert.AreEqual(false, model.SentToAdmin);
            Assert.IsNotNull(model.Form);
        }

        /// <summary>
        /// Validates that form is initialized when TempData contains a non-boolean value
        /// </summary>
        [Test]
        public void OnGet_TempData_Success_Not_Boolean_Sets_Form_Only()
        {
            // Arrange
            pageModel.TempData = TestHelper.TempData;
            pageModel.TempData["Success"] = "yes";

            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual(false, pageModel.SentToUser);
            Assert.AreEqual(false, pageModel.SentToAdmin);
            Assert.IsNotNull(pageModel.Form);
        }
        #endregion OnGet Tests
    }
}
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Threading.Tasks;

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
        #endregion

        #region OnPostAsync

        /// <summary>
        /// Validates that a valid form submission triggers both emails 
        /// and redirects
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task OnPostAsync_Valid_Form_Sends_Emails_And_Redirect()
        {
            // Arrange
            pageModel.TempData = TestHelper.TempData;
            pageModel.Form = new ContactFormModel
            {
                Name = "User",
                Email = "User@example.com",
                Message = "Test"
            };

            // Act
            var result = await pageModel.OnPostAsync();
            var redirectResult = (RedirectToPageResult)result;

            // Assert
            Assert.AreEqual(true, pageModel.SentToUser);
            Assert.AreEqual(true, pageModel.SentToAdmin);
            Assert.AreEqual(false, pageModel.IsFailed);
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual(true, pageModel.TempData.ContainsKey("Success"));
        }

        /// <summary>
        /// Validates that OnPost returns to the page when ModelState is invalid
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task OnPostAsync_Invalid_Model_State_Returns_Page()
        {
            // Arrange
            var settings = TestHelper.LoadEmailSettings();
            var emailService = new EmailService(settings);
            var model = new ContactModel(emailService);

            model.ModelState.AddModelError("Form Email", "Email is required");

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.AreEqual(false, model.SentToUser);
            Assert.AreEqual(false, model.SentToAdmin);
            Assert.AreEqual(false, model.IsFailed);
        }

        /// <summary>
        /// Validates that an SMTP failure triggers the IsFailed flag and returns to the page
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task OnPostAsync_Invalid_Smtp_Configuration_Triggers_Flag()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new EmailSettingsModel
            {
                SmtpServer = "invalid.smtp.server",
                Port = 2525,
                SenderName = "Fake",
                SenderEmail = "fake@example.com",
                Username = "wrong",
                Password = "wrong"
            });

            var emailService = new EmailService(options);
            var model = new ContactModel(emailService)
            {
                Form = new ContactFormModel
                {
                    Name = "User",
                    Email = "User@example.com",
                    Message = "Unit Test"
                }
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.AreEqual(false, model.SentToUser);
            Assert.AreEqual(false, model.SentToAdmin);
            Assert.AreEqual(true, model.IsFailed);
            Assert.IsInstanceOf<PageResult>(result);
        }
        #endregion 
    }
}
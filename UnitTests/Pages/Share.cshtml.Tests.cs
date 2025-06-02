using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UnitTests.Pages
{
    /// <summary>
    /// Provides unit testing for the Share page
    /// </summary>
    public class ShareTests
    {
        #region TestSetup

        // Page model for Share page
        public static ShareModel pageModel;

        // Email service used for testing
        public static EmailService emailService;

        /// <summary>
        /// Initializes the test environment with test email settings
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            var settings = TestHelper.LoadEmailSettings();
            emailService = new EmailService(settings);
            pageModel = new ShareModel(emailService);
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Validates that OnGet initializes the form with default message
        /// </summary>
        [Test]
        public void OnGet_Should_Initialize_Form_And_Default_Message()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.Form);
            Assert.IsFalse(string.IsNullOrEmpty(pageModel.Form.Message));
            StringAssert.Contains("Avengers Encyclopedia", pageModel.Form.Message);
        }

        #endregion OnGet Tests

        #region OnPostAsync Tests

        /// <summary>
        /// Validates that a valid form submission sends email successfully
        /// </summary>
        [Test]
        public async Task OnPostAsync_ValidForm_SendsEmail_SetsSentFlag()
        {
            // Arrange
            pageModel.Form = new ShareModel.ShareForm
            {
                Email = "test@example.com",
                Message = "Test message"
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsTrue(pageModel.SentToRecipient);
            Assert.IsFalse(pageModel.IsFailed);
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsEmpty(pageModel.Form.Email); // Email cleared after send
        }

        /// <summary>
        /// Validates that invalid ModelState returns the page without sending email
        /// </summary>
        [Test]
        public async Task OnPostAsync_InvalidModelState_ReturnsPageWithoutSending()
        {
            // Arrange
            pageModel.Form = new ShareModel.ShareForm
            {
                Email = "",  // Invalid email
                Message = "Test"
            };
            pageModel.ModelState.AddModelError("Form.Email", "Required");

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsFalse(pageModel.SentToRecipient);
            Assert.IsFalse(pageModel.IsFailed);
            Assert.IsInstanceOf<PageResult>(result);
        }

        /// <summary>
        /// Validates that email sending failure sets IsFailed flag and returns page
        /// </summary>
        [Test]
        public async Task OnPostAsync_EmailSendFailure_SetsIsFailedFlag()
        {
            // Arrange
            // Create EmailService with invalid SMTP config to simulate failure
            var badSettings = Microsoft.Extensions.Options.Options.Create(new EmailSettingsModel
            {
                SmtpServer = "invalid.smtp.server",
                Port = 2525,
                SenderName = "Fake",
                SenderEmail = "fake@example.com",
                Username = "wrong",
                Password = "wrong"
            });
            var badEmailService = new EmailService(badSettings);
            var badModel = new ShareModel(badEmailService)
            {
                Form = new ShareModel.ShareForm
                {
                    Email = "recipient@example.com",
                    Message = "Test failure"
                }
            };

            // Act
            var result = await badModel.OnPostAsync();

            // Assert
            Assert.IsFalse(badModel.SentToRecipient);
            Assert.IsTrue(badModel.IsFailed);
            Assert.IsInstanceOf<PageResult>(result);
        }

        #endregion OnPostAsync Tests

    }
}

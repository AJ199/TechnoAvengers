using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UnitTests.Pages.Services
{
    /// <summary>
    /// Unit tests for EmailService
    /// </summary>
    public class EmailServiceTests
    {
        #region SetUp
        // EmailService instance
        private EmailService EmailService;

        /// <summary>
        /// Initializes the service before each test with test configuration
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var options = TestHelper.LoadEmailSettings();
            EmailService = new EmailService(options);
        }

        #endregion

        #region SendEmailAsync
        /// <summary>
        /// Verifies SendEmailAsync executes without exceptions when provided with 
        /// valid input and configuration
        /// </summary>
        [Test]
        public async Task SendEmailAsync_Valid_Inputs__Should_Not_Throw_Exception()
        {
            // Arrange
            var to = "test@example.com"; // Valid test recipient
            var subject = "Test Subject";
            var body = "Unit Test";

            // Act
            var result = EmailService.SendEmailAsync(to, subject, body);


            // Assert
            Assert.DoesNotThrowAsync(async () => await result);

        }

        /// <summary>
        /// Verifies SendEmailAsync throws an exception when configured with invalid SMTP server details
        /// </summary>
        [Test]
        public void SendEmailAsync_Invalid_Test_Smtp_Configuration_Should_Throw_Exception()
        {
            // Arrange: invalid SMTP config
            var options = Options.Create(new EmailSettingsModel
            {
                SmtpServer = "invalid.smtp.server",
                Port = 2525,
                Username = "invalid",
                Password = "invalid",
                SenderEmail = "fake@domain.com",
                SenderName = "Fake Sender"
            });

            var service = new EmailService(options);

            // Act

            var result = service.SendEmailAsync("user@example.com", "Should Fail", "This should not send");

            // Assert
            Assert.ThrowsAsync<SmtpException>(async () => await result);
        }
        #endregion
    }
}
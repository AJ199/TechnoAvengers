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

    }
}

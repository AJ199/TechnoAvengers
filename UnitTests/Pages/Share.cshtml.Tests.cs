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

       
    }
}

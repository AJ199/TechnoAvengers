using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// PageModel class for the Share page.
    /// Handles the logic for sharing the Avengers Encyclopedia site via email.
    /// </summary>
    public class ShareModel : PageModel
    {
        /// <summary>
        /// Binds form input fields for sharing.
        /// </summary>
        [BindProperty]
        public ShareForm Form { get; set; }

        /// <summary>
        /// Flag indicating if the email was successfully sent to the recipient.
        /// </summary>
        public bool SentToRecipient { get; set; } = false;

        /// <summary>
        /// Flag indicating if sending the email failed.
        /// </summary>
        public bool IsFailed { get; set; } = false;

        /// <summary>
        /// Service used to send emails.
        /// </summary>
        private readonly EmailService EmailService;

        /// <summary>
        /// Constructor that accepts an EmailService instance via dependency injection.
        /// </summary>
        /// <param name="emailService">Service for sending emails</param>
        public ShareModel(EmailService emailService)
        {
            EmailService = emailService;
        }

        /// <summary>
        /// Class representing the form data for sharing.
        /// </summary>
        public class ShareForm
        {
            /// <summary>
            /// Recipient's email address.
            /// Required and must be a valid email format.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            /// Message to be sent in the email.
            /// Required field.
            /// </summary>
            [Required]
            [Display(Name = "Message")]
            public string Message { get; set; }
        }

        /// <summary>
        /// Handles HTTP GET requests.
        /// Initializes the form with a default sharing message if empty.
        /// </summary>
        public void OnGet()
        {
            // Create new form instance
            Form = new ShareForm();

            // If Message field is empty, prefill with a default message
            if (string.IsNullOrEmpty(Form.Message))
            {
                Form.Message = @"Hey there!

I just found this awesome Avengers Encyclopedia site and thought you’d love it too.

It’s a beautifully designed fan-made site packed with detailed profiles of all Marvel heroes — from Iron Man to Black Widow — along with their origin stories, powers, team-ups, and movies they appeared in.

If you’re into Marvel or know someone who is, this is a must-see!

Check it out here: https://5110technoavengersteam3.azurewebsites.net/

Let me know what you think!";
            }
        }

        /// <summary>
        /// Handles HTTP POST requests when the user submits the share form.
        /// Sends an email with the provided message to the specified recipient email address.
        /// </summary>
        /// <returns>
        /// Returns the current page with success or failure status.
        /// On success, clears the email input field.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate the form data
            if (ModelState.IsValid == false)
            {
                // If validation fails, redisplay the page with validation errors
                return Page();
            }

            // Email subject for the share email
            var subject = "Check out the Avengers Encyclopedia!";

            // Email body uses the message entered in the form
            var body = $"{Form.Message}";

            try
            {
                // Attempt to send email using the injected EmailService
                await EmailService.SendEmailAsync(Form.Email, subject, body);

                // Mark that the email was successfully sent
                SentToRecipient = true;

                // Clear only the email input field after successful send
                Form.Email = "";

                // Remove the email field from model state to avoid repopulating it on the page reload
                ModelState.Remove("Form.Email");

                // Redisplay the page to show success message and cleared email input
                return Page();
            }
            catch (Exception ex)
            {
                // If an error occurs during sending, mark failure flag and log the error
                IsFailed = true;

                // Redisplay the page showing failure message
                return Page();
            }
        }
    }
}
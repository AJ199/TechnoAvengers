using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Handles the logic for displaying the Contact Razor Page
    /// </summary>
    public class ContactModel : PageModel
    {
        // Service to send email
        private readonly EmailService EmailService;

        // Contact form input data
        [BindProperty]
        public ContactFormModel Form { get; set; }

        // Indicates email was sent to user
        public bool SentToUser { get; set; }

        // Indicates email was sent to admin
        public bool SentToAdmin { get; set; }

        // Indicates a failure sending an occurred
        public bool IsFailed { get; set; }

        /// <summary>
        /// Initiales ContactModel with a given EmailService
        /// </summary>
        /// <param name="emailService">Service used to send emails</param>
        public ContactModel(EmailService emailService)
        {
            EmailService = emailService;
        }

        /// <summary>
        /// Handles GET requests to load the Contact page
        /// </summary>
        public void OnGet()
        {
            // No success state is displayed if there was no successful POST submission before
            if (TempData.ContainsKey("Success") == false)
            {
                Form = new ContactFormModel();
                return;
            }

            // Prevents unsafe casting 
            if (TempData["Success"] is bool == false)
            {
                Form = new ContactFormModel();
                return;
            }

            // Retrieve TempData value
            var valueTemptData = TempData["Success"];

            // Cast the value to a boolean
            var successFlag = (bool)valueTemptData;

            // Update the status indicators used to display success message
            if (successFlag == true)
            {
                SentToUser = true;
                SentToAdmin = true;
            }

            // Create a fresh, empty form
            Form = new ContactFormModel();
        }

        /// <summary>
        /// Handles POST requests from the Contact form
        /// </summary>
        /// <returns>If successful, redirect; otherwise, the same page </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            // Admin email
            var adminEmail = "avengers.encyclopedia@gmail.com";

            // Email subject for user email
            string userSubject = "Thanks for contacting the Avengers Encyclopedia!";

            // Emal body for user email
            string userBody = $"Hi {Form.Name},\n\nThank you for your feedback. " +
                              $"We highly appreciate your input. Our team will consider your message carefully.\n\n" +
                              $"- The Avengers Encyclopedia Team";

            // Email subject for admin email
            string adminSubject = $"New Contact Form from {Form.Name}";

            // Email body for admin email
            string adminBody = $"Name: {Form.Name}\nEmail: {Form.Email}\n\nMessage:\n{Form.Message}";

            try
            {
                // Send email to user
                await EmailService.SendEmailAsync(Form.Email, userSubject, userBody);

                SentToUser = true;

                // Send email to admin
                await EmailService.SendEmailAsync(adminEmail, adminSubject, adminBody);
                SentToAdmin = true;

                // Store success state
                TempData["Success"] = true;

                return RedirectToPage();
            }
            catch
            {
                IsFailed = true;
            }

            return Page();
        }
    }
}
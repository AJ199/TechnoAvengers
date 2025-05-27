using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Handles the logic for displaying the Contact Razor Page.
    /// </summary>
    public class ContactModel : PageModel
    {
        /// <summary>
        /// Handles GET requests to load the Contact page
        /// </summary>
        public void OnGet()
        {
            // No data processing is needed
        }

        /// <summary>
        /// Handles POST requests from the Contact form
        /// </summary>
        /// <returns>If successful, redirect; otherwise, the same page </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid == false)
                return Page();

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
                await EmailService.SendEmailAsync("avengers.encyclopedia@gmail.com", adminSubject, adminBody);
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
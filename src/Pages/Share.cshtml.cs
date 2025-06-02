using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System;
using ContosoCrafts.WebSite.Services;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Pages
{
    public class ShareModel : PageModel
    {
        [BindProperty]
        public ShareForm Form { get; set; }

        public bool SentToRecipient { get; set; } = false;
        public bool IsFailed { get; set; } = false;

        private readonly EmailService EmailService;

        public ShareModel(EmailService emailService)
        {
            EmailService = emailService;
        }

        public class ShareForm
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Message")]
            public string Message { get; set; }
        }


        public void OnGet()
        {
            Form = new ShareForm();

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


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var subject = "Check out the Avengers Encyclopedia!";
            var body = $"{Form.Message}";


            try
            {
                // Send the email via your existing EmailService
                await EmailService.SendEmailAsync(Form.Email, subject, body);
                SentToRecipient = true;

                return RedirectToPage(); // Or RedirectToPage("SuccessPage") if needed
            }
            catch (Exception ex)
            {
                IsFailed = true;
                Console.WriteLine("Email sending failed: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                return Page();
            }
        }
    }
}

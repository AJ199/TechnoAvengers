using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents SMTP settings loaded from configuration
    /// </summary>
    public class EmailSettingsModel
    {
        // SMTP server address
        [Required(ErrorMessage = "SMTP server is required")]
        public string SmtpServer { get; set; }

        // SMTP port
        [Required(ErrorMessage = "Port number is required")]
        public int Port { get; set; }

        // Display name of the sender
        [Required(ErrorMessage = "Sender name is required")]
        public string SenderName { get; set; }

        // Sender's email address
        [Required(ErrorMessage = "Sender email is required")]
        [EmailAddress(ErrorMessage = "Sender email must be a valid email address")]
        public string SenderEmail { get; set; }

        // Username for SMTP authentication
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        // Password for SMTP authentication
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
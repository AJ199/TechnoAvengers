namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents the configuration for sending emails
    /// </summary>
    public class EmailModel
    {
        // The SMTP server address
        public string SmtpServer { get; set; }

        // The port number for the SMTP server
        public int Port { get; set; }

        // The name of the sender
        public string SenderName { get; set; }

        // The email address of the sender
        public string SenderEmail { get; set; }

        // The username for authenticating with the SMTP server
        public string Username { get; set; }

        // The password for authenticating with the SMTP server
        public string Password { get; set; }
    }
}
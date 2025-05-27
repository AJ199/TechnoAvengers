using ContosoCrafts.WebSite.Models;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Provides email delivery by using SMTP settings provided through 
    /// application configuration
    /// </summary>
    public class EmailService
    {
        // Email configuration settings
        private readonly EmailSettingsModel Settings;

        /// <summary>
        /// Initializes an EmailService instances with the given settings
        /// </summary>
        /// <param name="options">Settings from configuration</param>
        public EmailService(IOptions<EmailSettingsModel> options)
        {
            Settings = options.Value;
        }

        /// <summary>
        /// Sends an email to the specified recipient with the given subject and body
        /// </summary>
        /// <param name="to">Recipient's email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email message</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // SMTP client configuration
            var smtpClient = new SmtpClient
            {
                Host = Settings.SmtpServer,
                Port = Settings.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(Settings.Username, Settings.Password)
            };

            // Build the email message using the input and settings values
            var message = new MailMessage
            {
                From = new MailAddress(Settings.SenderEmail, Settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            // Add the recipient to the message
            message.To.Add(to);

            // Send the message using the configured SMTP client
            await smtpClient.SendMailAsync(message);
        }
    }
}
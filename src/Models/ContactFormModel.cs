using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents the contact form data submitted by the user
    /// </summary>
    public class ContactFormModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents the contact form data submitted by the user
    /// </summary>
    public class ContactFormModel
    {
        // The name of the person submitting the contact form
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Must be 50 characters or fewer")]
        [RegularExpression(@"^[a-zA-Z\s\-'\.]+$",
            ErrorMessage = "Use only letters, spaces, hyphens, apostrophes, and periods")]
        public string Name { get; set; }

        // The email address of the sender; must be valid and end with .com or .edu
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|edu)$", ErrorMessage = "Only .com and .edu email addresses are accepted")]
        public string Email { get; set; }

        // Message content, max 300 chars
        [Required(ErrorMessage = "Required")]
        [StringLength(300, ErrorMessage = "Message must be 300 characters or fewer")]
        public string Message { get; set; }
    }
}
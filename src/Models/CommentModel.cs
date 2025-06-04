using System.ComponentModel.DataAnnotations;
using System;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents a comment made by a user on a superhero read page
    /// </summary>
    public class CommentModel
    {
        // Unique identifier for this comment instance
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // ID of the superhero this comment belongs to
        public string SuperheroId { get; set; }

        // Username of the commenter
        [Required(ErrorMessage = "Alias is required")]
        [StringLength(30, ErrorMessage = "Alias must be 30 characters or fewer")]
        public string Username { get; set; }

        // Message content of the comment
        [Required(ErrorMessage = "Comment is required")]
        [StringLength(200, ErrorMessage = "Comment must be 200 characters or fewer")]
        public string Message { get; set; }

        // Number of likes the comment has received
        public int Likes { get; set; } = 0;
    }
}
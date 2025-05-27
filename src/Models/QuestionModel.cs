using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Respresents a question of the knowledge quiz
    /// </summary>
    public class QuestionModel
    {
        // Question 
        [Required]
        public string Question { get; set; }

        // Options the user can choose from
        [Required]
        public string[] Options { get; set; }

        // Correct answer
        [Required]
        public string Answer { get; set; }
    }
}
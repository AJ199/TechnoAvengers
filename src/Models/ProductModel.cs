using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Class for managing superhero data
    /// </summary>
    public class ProductModel
    {
        // Unique identifier for superhero
        public string? Id { get; set; }

        // Title represents superhero name, is required and must be 2-50 characters long
        [Required(ErrorMessage = "Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be 2-50 chars")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-':,!.&()]+$",
            ErrorMessage = "Title can contain letters, numbers, spaces, and basic punctuation")]
        public string? Title { get; set; }

        // Fullname represents superhero real name and must contain max 50 characters
        [StringLength(50, ErrorMessage ="50 chars max")]
        public string? Fullname { get; set; }

        // Birthplace of superhero must contain max 100 characters 
        [StringLength(100, ErrorMessage = "100 chars max")]
        public string? Birthplace { get; set; }

        // Work of superhero must contain max 150 characters
        [StringLength(150, ErrorMessage = "150 chars max")]
        public string? Work { get; set; }

        // FirstAppear is the first comic the superhero was included and must contain max 100 characters
        [StringLength(100, ErrorMessage = "100 chars max")]
        public string? FirstAppear { get; set; }

        // ImageUrl must be valid, https, and end with .jpg or .png extension
        [Required(ErrorMessage = "Required")]
        [Url(ErrorMessage = "Enter a valid URL")]
        [RegularExpression(@"^https:\/\/.+\/[^\/]+\.(jpg|jpeg|png)(\?.*)?$",
            ErrorMessage = "Must be a valid image URL (.jpg, .jpeg, .png) starting with https://")]
        public string? ImageUrl { get; set; }

        // Intelligence is a required power statistic and must be bewteen 0 and 100
        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "Must be 1-100")]
        public int Intelligence { get; set; }

        // Strength is a required power statistic and must be bewteen 0 and 100
        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "Must be 1-100")]
        public int Strength { get; set; }

        // Speed is a required power statistic and must be bewteen 0 and 100
        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "Must be 1-100")]
        public int Speed { get; set; }

        // Durability is a required power statistic and must be bewteen 0 and 100
        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "Must be 1-100")]
        public int Durability { get; set; }

        // Power is a required power statistic and must be bewteen 0 and 100
        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "Must be 1-100")]
        public int Power { get; set; }

        // Combat is a required power statistic and must be bewteen 0 and 100
        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "Must be 1-100")]
        public int Combat { get; set; }

        // Alignment represents the moral stance of the superhero (e.g., good, bad, neutral); max 100 characters
        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "30 chars max")]
        public string? Alignment { get; set; }

        // Role indicates the superhero's primary function in a team (e.g., Core, Support, Ally); max 100 characters
        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "30 chars max")]
        public string? Role { get; set; }

        // Gender specifies the biological or identified gender of the superhero (e.g., Male, Female, Other); max 100 characters
        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "30 chars max")]
        public string? Gender { get; set; }

        // Array of ratings
        public int[]? Ratings { get; set; }

        /// <summary>
        /// Converts the object to a JSON string
        /// </summary>
        /// <returns>Serialized JSON string</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize<ProductModel>(this);
        }
    }
}
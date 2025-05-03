using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
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

        // Title must be 2-50 characters long
        [Required(ErrorMessage = "Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "2-50 chars")]
        public string? Title { get; set; }

        // Fullname must contain max 50 characters
        [StringLength(50, ErrorMessage ="50 chars max")]
        public string? Fullname { get; set; }

        // Birthplace must contain max 100 characters 
        [StringLength(100, ErrorMessage = "100 chars max")]
        public string? Birthplace { get; set; }

        // Work must contain max 150 characters
        [StringLength(150, ErrorMessage = "150 chars max")]
        public string? Work { get; set; }

        // FirstAppear must contain max 100 characters 
        [StringLength(100, ErrorMessage = "100 chars max")]
        public string? FirstAppear { get; set; }

        // ImageUrl must be valid, https, and end with .jpg or .png extension
        [JsonPropertyName("ImageUrl")]
        [Required(ErrorMessage = "Required")]
        [Url(ErrorMessage = "Enter a valid URL")]
        [RegularExpression(@"^https:\/\/.*\.(jpg|png)$",
            ErrorMessage = "Must start with 'https://', end with .jpg or .png")]
        public string? ImageUrl { get; set; }

        // Dictionary of power statistics (intelligence, strength, speed, durability, power, combat)
        [JsonPropertyName("Powerstats")]
        public Dictionary<string, string>? Powerstats { get; set; }

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

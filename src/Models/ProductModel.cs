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

        // Title is required, must be alphanumeric and 3-40 characters long
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]{3,40}$", 
            ErrorMessage = "Only letters and numbers, 3-40 chars")]
        public string? Title { get; set; }

        // Fullname must contain alphabetic characters and 3-40 characters
        [RegularExpression(@"^[a-zA-Z\s]{3,40}$", 
            ErrorMessage = "Only letters, 3-40 chars")]
        public string? Fullname { get; set; }

        // Birthplace must contain alphabetic characters and 60 characters max
        [RegularExpression(@"^[a-zA-Z\s]{,60}$", 
            ErrorMessage = "Only letters, 60 chars max")]
        public string? Birthplace { get; set; }

        // Work must contain alphabetic characters and 80 characters max
        [RegularExpression(@"^[a-zA-Z\s]{,80}$", 
            ErrorMessage = "Only letters, 80 chars max")]
        public string? Work { get; set; }

        // FirstAppear must be alphanumeric and maximum 40 characters long
        [RegularExpression(@"^[a-zA-Z0-9\s]{,40}$", 
            ErrorMessage = "Only letters and numbers, 40 chars max")]
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

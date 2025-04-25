using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    public class ProductModel
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Fullname { get; set; }
        public string? Birthplace { get; set; }
        public string? Work { get; set; }
        public string? FirstAppear { get; set; }


        [JsonPropertyName("ImageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("Powerstats")]
        public Dictionary<string, string>? Powerstats { get; set; }
        public int[]? Ratings { get; set; }

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);
    }
}

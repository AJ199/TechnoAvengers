using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");

        public IEnumerable<ProductModel> GetProducts()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        // Alias method for backward compatibility with other parts of the codebase
        public IEnumerable<ProductModel> GetAllData() => GetProducts();

        public void AddRating(string productId, int rating)
        {
            var products = GetProducts().ToList();

            var product = products.FirstOrDefault(x => x.Id == productId);
            if (product == null)
            {
                return;
            }

            if (product.Ratings == null)
            {
                product.Ratings = new int[] { rating };
            }
            else
            {
                var ratings = product.Ratings.ToList();
                ratings.Add(rating);
                product.Ratings = ratings.ToArray();
            }

            SaveData(products);
        }

        public void SaveData(IEnumerable<ProductModel> products)
        {
            using var outputStream = File.Create(JsonFileName);
            JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true,
                    Indented = true
                }),
                products
            );
        }
    }
}

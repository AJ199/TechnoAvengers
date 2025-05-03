using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Service class to manage superhero data stored in JSON format
    /// </summary>
    public class JsonFileProductService
    {
        /// <summary>
        /// Initializes a new JsonFileProductService instance 
        /// </summary>
        /// <param name="webHostEnvironment">Hosting environment information</param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Retrieves the web hosting environment
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Gets the full path to the JSON file containing the category data.
        /// </summary>
        private string GetJsonFileName()
        {
            return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");
        }

        /// <summary>
        /// Retrieves all superhero data from the JSON file
        /// </summary>
        /// <returns>A collection of ProductModel objects</returns>
        public IEnumerable<ProductModel> GetAllData()
        {
            // Opens the JSON file
            using (var jsonFileReader = File.OpenText(GetJsonFileName()))
            { 
                // JSON parsing rules
                JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserialized JSON data
                ProductModel[]? products = JsonSerializer.Deserialize<ProductModel[]>
                    (jsonFileReader.ReadToEnd(), serializerOptions);

                if (products == null) 
                { 
                    return Enumerable.Empty<ProductModel>();
                }

                return products;
            }
        }

        /// <summary>
        /// Adds a rating to the superhero with the given ID
        /// </summary>
        /// <param name="productId">ID associated with superhero</param>
        /// <param name="rating">Rating to be added</param>
        /// <returns></returns>
        public bool AddRating(string productId, int rating)
        {
            // Retrieves all data
            var products = GetAllData();

            // Finds superhero with given ID
            var product = products.First(x => x.Id == productId);

            if (product == null)
            {
                return false;
            }

            if (product.Ratings == null)
            {
                product.Ratings = new int[] { rating };
            }
            else
            {
                // Converts ratings to list
                var ratings = product.Ratings.ToList();
                ratings.Add(rating);
                product.Ratings = ratings.ToArray();
            }

            // Creates a file stream to save the serialized data
            using var outputStream = File.OpenWrite(GetJsonFileName());

            JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true,
                    Indented = true
                }),
                products
            );

            return true;
        }

        /// <summary>
        /// Updates superhero data, except its ID
        /// </summary>
        /// <param name="data">Data to update</param>
        /// <returns>True if succesfully updated, false otherwise</returns>
        public bool UpdateData(ProductModel data)
        {
            // Gets all data
            IEnumerable<ProductModel> products = GetAllData();

            // Finds superhero to update
            ProductModel? productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));

            if (productData == null)
            {
                return false;
            }

            // Update properties except ID
            productData.Title = data.Title;
            productData.Fullname = data.Fullname;
            productData.Birthplace = data.Birthplace;
            productData.Work = data.Work;
            productData.FirstAppear = data.FirstAppear;
            productData.ImageUrl = data.ImageUrl;
            productData.Powerstats = data.Powerstats;
            productData.Ratings = data.Ratings;

            SaveData(products);
            return true;
        }

        /// <summary>
        /// Saves the given list to a JSON file
        /// </summary>
        /// <param name="products">List of superheroes</param>
        private void SaveData(IEnumerable<ProductModel> products)
        {
            // Creates a file stream to save the serialized data
            using (var outputStream = File.Create(GetJsonFileName()))
            {
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
}

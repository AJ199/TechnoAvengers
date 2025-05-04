using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Provides access to data stored in JSON format
    /// </summary>
    public class JsonFileProductService
    {
        /// <summary>
        /// Web root path is retrieved through the hosting environment
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Initializes a new JSON product service instance 
        /// </summary>
        /// <param name="webHostEnvironment">Hosting environment</param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Retrieves the path to the JSON file
        /// </summary>
        private string JsonFileName
        {
            get
            {
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");
            }
        }

        /// <summary>
        /// Retrieves all superhero data from the JSON file
        /// </summary>
        /// <returns>A collection of ProductModel objects</returns>
        public IEnumerable<ProductModel> GetProducts()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);

            // JSON parsing rules 
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserialized JSON data
            ProductModel[]? products = JsonSerializer.Deserialize<ProductModel[]>(
                jsonFileReader.ReadToEnd(),
                serializerOptions);

            if (products == null)
            {
                return Array.Empty<ProductModel>();
            }

            return products;
        }

        /// <summary>
        /// Adds a rating to the superhero with the given ID
        /// </summary>
        /// <param name="productId">ID associated with superhero</param>
        /// <param name="rating">Rating to be added</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates superhero data, except its ID
        /// </summary>
        /// <param name="data">Data to update</param>
        /// <returns>True if successfully updated, false otherwise</returns>
        public bool UpdateData(ProductModel data)
        {
            var products = GetProducts().ToList();
            var productData = products.FirstOrDefault(x => x.Id == data.Id);

            if (productData == null)
            {
                return false;
            }

            // Required properties
            productData.Title = data.Title;
            productData.ImageUrl = data.ImageUrl;

            // Required Power statistics 
            productData.Intelligence = data.Intelligence;
            productData.Strength = data.Strength;
            productData.Speed = data.Speed;
            productData.Durability = data.Durability;
            productData.Power = data.Power;
            productData.Combat = data.Combat;

            // Apply "-" for optional properties if empty
            productData.Fullname = string.IsNullOrWhiteSpace(data.Fullname) ? "-" : data.Fullname;
            productData.Birthplace = string.IsNullOrWhiteSpace(data.Birthplace) ? "-" : data.Birthplace;
            productData.Work = string.IsNullOrWhiteSpace(data.Work) ? "-" : data.Work;
            productData.FirstAppear = string.IsNullOrWhiteSpace(data.FirstAppear) ? "-" : data.FirstAppear;
            
            productData.Ratings = data.Ratings;

            SaveData(products);
            return true;
        }

        /// <summary>
        /// Saves the given list to a JSON file
        /// </summary>
        /// <param name="products">List of superheroes</param>
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

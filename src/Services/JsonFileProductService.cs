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
    /// Provides access to product data stored in JSON format and supports basic CRUD operations.
    /// </summary>
    public class JsonFileProductService
    {
        /// <summary>
        /// Provides access to the web root path through the hosting environment.
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Initializes a new instance of the JsonFileProductService class using dependency injection.
        /// </summary>
        /// <param name="webHostEnvironment">The current hosting environment.</param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Constructs the full path to the JSON file where products are stored.
        /// </summary>
        private string JsonFileName
        {
            get
            {
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");
            }
        }

        /// <summary>
        /// Retrieves and deserializes all product data from the JSON file.
        /// </summary>
        /// <returns>A collection of ProductModel objects; returns an empty array if the file is empty or invalid.</returns>
        public IEnumerable<ProductModel> GetProducts()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);

            JsonSerializerOptions serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            ProductModel[]? products = JsonSerializer.Deserialize<ProductModel[]>(
                jsonFileReader.ReadToEnd(),
                serializerOptions);

            // Return an empty list if deserialization failed or file is empty
            return products ?? Array.Empty<ProductModel>();
        }

        /// <summary>
        /// Adds a new rating to the product with the specified ID.
        /// </summary>
        /// <param name="productId">The ID of the product to be rated.</param>
        /// <param name="rating">The rating to add (e.g., from 1 to 5).</param>
        public void AddRating(string productId, int rating)
        {
            var products = GetProducts().ToList();

            // Find the product by ID
            var product = products.FirstOrDefault(x => x.Id == productId);
            if (product == null)
            {
                return; // Do nothing if product is not found
            }

            // Initialize ratings if null, otherwise append the new rating
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

            // Persist the updated product list
            SaveData(products);
        }

        /// <summary>
        /// Updates an existing product's information, keeping the ID unchanged.
        /// </summary>
        /// <param name="data">The updated product data.</param>
        /// <returns>True if update was successful; false if the product was not found.</returns>
        public bool UpdateData(ProductModel data)
        {
            var products = GetProducts().ToList();
            var productData = products.FirstOrDefault(x => x.Id == data.Id);

            if (productData == null)
            {
                return false; // Product with matching ID not found
            }

            // Update required fields
            productData.Title = data.Title;
            productData.ImageUrl = data.ImageUrl;
            productData.Alignment = data.Alignment;
            productData.Gender = data.Gender;
            productData.Role = data.Role;

            // Update core power attributes
            productData.Intelligence = data.Intelligence;
            productData.Strength = data.Strength;
            productData.Speed = data.Speed;
            productData.Durability = data.Durability;
            productData.Power = data.Power;
            productData.Combat = data.Combat;

            // Use placeholder "-" for optional fields if they are blank
            productData.Fullname = string.IsNullOrWhiteSpace(data.Fullname) ? "-" : data.Fullname;
            productData.Birthplace = string.IsNullOrWhiteSpace(data.Birthplace) ? "-" : data.Birthplace;
            productData.Work = string.IsNullOrWhiteSpace(data.Work) ? "-" : data.Work;
            productData.FirstAppear = string.IsNullOrWhiteSpace(data.FirstAppear) ? "-" : data.FirstAppear;

            // Replace ratings
            productData.Ratings = data.Ratings;

            // Save updated product list
            SaveData(products);
            return true;
        }

        /// <summary>
        /// Adds a new product to the JSON file. Optional fields are defaulted if left blank.
        /// </summary>
        /// <param name="data">The new product to add.</param>
        /// <returns>True if the product was successfully created; false if a product with the same ID already exists.</returns>
        public bool CreateData(ProductModel data)
        {
            if (data == null)
            {
                return false;
            }

            var products = GetProducts().ToList();
            var existing = products.FirstOrDefault(p => p.Id == data.Id);

            // Only add if the product ID is unique
            if (existing == null)
            {
                // Set defaults for optional properties if missing
                string fullname = string.IsNullOrWhiteSpace(data.Fullname) ? "-" : data.Fullname;
                string birthplace = string.IsNullOrWhiteSpace(data.Birthplace) ? "-" : data.Birthplace;
                string work = string.IsNullOrWhiteSpace(data.Work) ? "-" : data.Work;
                string firstAppear = string.IsNullOrWhiteSpace(data.FirstAppear) ? "-" : data.FirstAppear;

                // Create a new product with all values set
                ProductModel newProduct = new ProductModel
                {
                    Id = data.Id,
                    Title = data.Title,
                    ImageUrl = data.ImageUrl,
                    Intelligence = data.Intelligence,
                    Strength = data.Strength,
                    Speed = data.Speed,
                    Durability = data.Durability,
                    Power = data.Power,
                    Combat = data.Combat,
                    Fullname = fullname,
                    Birthplace = birthplace,
                    Work = work,
                    FirstAppear = firstAppear,
                    Ratings = data.Ratings,
                    Role = data.Role,
                    Gender = data.Gender,
                    Alignment = data.Alignment
                };

                products.Add(newProduct);
                SaveData(products);
                return true;
            }

            return false; // Product already exists
        }

        /// <summary>
        /// Saves the current list of products to the JSON file, overwriting existing content.
        /// </summary>
        /// <param name="products">The updated product list to save.</param>
        public void SaveData(IEnumerable<ProductModel> products)
        {
            using var outputStream = File.Create(JsonFileName);
            JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                new Utf8JsonWriter(outputStream, new JsonWriterOptions
                {
                    SkipValidation = true,
                    Indented = true // Format the JSON for readability
                }),
                products
            );
        }
    }
}

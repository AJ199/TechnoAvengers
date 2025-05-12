using System.Collections.Generic;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Retrieves superhero data and filters it based in query parameters
    /// </summary>
    public class IndexModel : PageModel
    {
        // Keeps track of application behavior and errors
        private readonly ILogger<IndexModel> _logger;

        // Retrieves superhero data from the JSON file
        public JsonFileProductService ProductService { get; }

        // Filtered list of superheroes based on selected values
        public List<ProductModel> FilteredHeroes { get; set; }

        /// <summary>
        /// Initializes a new instance of the IndexModel class
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="productService">Service for accessing superhero data</param>
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        /// <summary>
        /// Handles the GET request
        /// Retrieves, filters and saves the superheroes in FilteredHeroes property
        /// </summary>
        public void OnGet()
        {
            // All data from JSON file
            var allHeroes = ProductService.GetProducts();

            // Retrieves the user's filter choices for Alignment category
            var selectedAlignments = Request.Query["Alignment"].ToList();

            // Retrieves the user's filter choices for Role category
            var selectedRoles = Request.Query["Role"].ToList();

            // Retrieves the user's filter choices for Gender category
            var selectedGenders = Request.Query["Gender"].ToList();

            // Minimun value chosen by user for Intelligence
            int minIntelligence = GetMin("Intelligence");

            // Maximum value chosen by user for Intelligence
            int maxIntelligence = GetMax("Intelligence");

            // Minimun value chosen by user for Strength
            int minStrength = GetMin("Strength");

            // Maximum value chosen by user for Strength
            int maxStrength = GetMax("Strength");

            // Minimun value chosen by user for Speed
            int minSpeed = GetMin("Speed");

            // Maximum value chosen by user for Speed
            int maxSpeed = GetMax("Speed");

            // Minimun value chosen by user for Durability
            int minDurability = GetMin("Durability");

            // Maximum value chosen by user for Durability
            int maxDurability = GetMax("Durability");

            // Minimun value chosen by user for Power
            int minPower = GetMin("Power");

            // Maximum value chosen by user for Durability
            int maxPower = GetMax("Power");

            // Minimun value chosen by user for Combat
            int minCombat = GetMin("Combat");

            // Maximum value chosen by user for Durability
            int maxCombat = GetMax("Combat");


            // Filters the superheroes based on the selected parameters, 
            // if no filters are selected, all superheroes are included
            FilteredHeroes = allHeroes
                .Where(hero =>
                    (selectedAlignments.Count == 0 || selectedAlignments.Contains(hero.Alignment)) &&
                    (selectedRoles.Count == 0 || selectedRoles.Contains(hero.Role)) &&
                    (selectedGenders.Count == 0 || selectedGenders.Contains(hero.Gender)) &&

                    hero.Intelligence >= minIntelligence && hero.Intelligence <= maxIntelligence &&
                    hero.Strength >= minStrength && hero.Strength <= maxStrength &&
                    hero.Speed >= minSpeed && hero.Speed <= maxSpeed &&
                    hero.Durability >= minDurability && hero.Durability <= maxDurability &&
                    hero.Power >= minPower && hero.Power <= maxPower &&
                    hero.Combat >= minCombat && hero.Combat <= maxCombat
                ).ToList();
        }

        /// <summary>
        /// Retrieves the list of possible values for a given category
        /// </summary>
        /// <param name="category">Name of category</param>
        /// <returns>List of strings representing values for the given category</returns>
        public List<string> GetValuesForCategory(string category)
        {
            List<string> values = new List<string>();

            // Use a switch statement to populate the list based on category
            switch (category)
            {
                case "Alignment":
                {
                    values = new List<string> { "good", "bad", "neutral" };
                    break;
                }

                case "Gender":
                {
                    values = new List<string> { "Male", "Female", "Other" };
                    break;
                }

                case "Role":
                {
                    values = new List<string> { "Core Avenger", "Founding Avenger", "Mystic Defender", "Guardian", "Support", "Unknown" };
                    break;
                }
            }

            return values;
        }

        /// <summary>
        /// Retrieves the minimum value for a given power stat from the query
        /// Returns 1 if the parameter is missing or invalid
        /// </summary>
        /// <param name="key">The stat name</param>
        /// <returns>The minimum value as an integer</returns>
        private int GetMin(string key)
        {
            // Holds the parsed query value
            int val;

            if (int.TryParse(Request.Query[$"{key}Min"], out val))
            {
                return val;
            }

            return 1;
        }

        /// <summary>
        /// Retrieves the maximum value for a given power stat from the query
        /// Returns 100 if the parameter is missing or invalid
        /// </summary>
        /// <param name="key">The stat name</param>
        /// <returns>The maximum value as an integer</returns>
        private int GetMax(string key)
        {
            // Holds the parsed query value
            int val;

            if (int.TryParse(Request.Query[$"{key}Max"], out val))
            {
                return val;
            }

            return 100;
        }


    }
}
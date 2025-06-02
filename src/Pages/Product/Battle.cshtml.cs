using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Manages the superhero battle page including hero selection, voting, results and comments.
    /// </summary>
    public class BattleModel : PageModel
    {
        /// <summary>
        /// Service used to access superhero data.
        /// </summary>
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor to inject the product service.
        /// </summary>
        /// <param name="productService">Service for accessing superhero data</param>
        public BattleModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// List of all available superheroes.
        /// </summary>
        public List<ProductModel> Products { get; set; } = new();

        /// <summary>
        /// ID of the first selected hero.
        /// </summary>
        [BindProperty]
        public string? Hero1Id { get; set; }

        /// <summary>
        /// ID of the second selected hero.
        /// </summary>
        [BindProperty]
        public string? Hero2Id { get; set; }

        /// <summary>
        /// First hero's full data.
        /// </summary>
        public ProductModel? Hero1 { get; set; }

        /// <summary>
        /// Second hero's full data.
        /// </summary>
        public ProductModel? Hero2 { get; set; }

        /// <summary>
        /// ID of the hero predicted to win.
        /// </summary>
        [BindProperty]
        public string? PredictedWinnerId { get; set; }

        /// <summary>
        /// Hero predicted to win.
        /// </summary>
        public ProductModel? PredictedWinner { get; set; }

        /// <summary>
        /// The actual winner hero.
        /// </summary>
        public ProductModel? ActualWinner { get; set; }

        /// <summary>
        /// The hero who lost.
        /// </summary>
        public ProductModel? Loser { get; set; }

        /// <summary>
        /// Message showing whether prediction was correct.
        /// </summary>
        public string? ResultMessage { get; set; }

        /// <summary>
        /// List of selectable hero options for dropdown.
        /// </summary>
        public List<SelectListItem> HeroOptions { get; set; } = new();

        /// <summary>
        /// Current step in the battle workflow.
        /// </summary>
        [BindProperty]
        public BattleStep Step { get; set; } = BattleStep.SelectHeroes;

        /// <summary>
        /// Enum representing the current step of the battle process.
        /// </summary>
        public enum BattleStep
        {
            SelectHeroes = 1,   // Selecting heroes
            VoteWinner = 2,     // Voting on winner
            ShowResult = 3      // Displaying result
        }

        /// <summary>
        /// Returns the average rating of the hero if ratings exist; otherwise, returns 0.
        /// </summary>
        public double GetHeroRating(ProductModel hero)
        {
            if (hero.Ratings.Length > 0)
            {
                return hero.Ratings.Average();
            }
            return 0;
        }

        /// <summary>
        /// Handles GET requests to initialize the page.
        /// </summary>
        public void OnGet()
        {
            Products = _productService.GetProducts().ToList();

            HeroOptions = Products.Select(p => new SelectListItem
            {
                Value = p.Id,
                Text = p.Title
            }).ToList();
        }

        /// <summary>
        /// Handles POST requests for all steps: selecting heroes, voting, showing results, and adding comments.
        /// </summary>
        /// <returns>The page with updated model</returns>
        public IActionResult OnPost()
        {
            // Retrieve the full list of superheroes from the product service
            Products = _productService.GetProducts().ToList();

            // Create a list of selectable hero options for dropdown menus,
            // mapping each hero's ID to the option value and the hero's title to the display text
            HeroOptions = Products.Select(p => new SelectListItem
            {
                Value = p.Id,
                Text = p.Title
            }).ToList();

            // Check if the current battle step is hero selection
            if (Step == BattleStep.SelectHeroes)
            {
                // Validate that two different heroes have been selected and neither is empty or null
                var isInvalid = string.IsNullOrEmpty(Hero1Id) || string.IsNullOrEmpty(Hero2Id) || Hero1Id == Hero2Id;
                if (isInvalid)
                {
                    // Add a validation error to prompt user to select two distinct heroes
                    ModelState.AddModelError(string.Empty, "Please select two different heroes.");
                    // Return the page with the error message displayed, preventing progress to next step
                    return Page();
                }

                // Load full data for each selected hero for use in the next step (voting)
                Hero1 = Products.FirstOrDefault(p => p.Id == Hero1Id);
                Hero2 = Products.FirstOrDefault(p => p.Id == Hero2Id);

                // Advance the battle step to voting for the winner
                Step = BattleStep.VoteWinner;

                // Return the page to re-render with updated data and UI state
                return Page();
            }


            // Handle VoteWinner step
            if (Step == BattleStep.VoteWinner)
            {
                // Check if the user failed to select a predicted winner
                var predictionMissing = string.IsNullOrEmpty(PredictedWinnerId);

                // If no prediction was made, show validation error and reload the selection page
                if (predictionMissing)
                {
                    // Add a validation error to the model state to be displayed on the page
                    ModelState.AddModelError(string.Empty, "Please select who you think will win.");

                    // Reload hero data so the page can re-render the same selected heroes
                    Hero1 = Products.FirstOrDefault(p => p.Id == Hero1Id);
                    Hero2 = Products.FirstOrDefault(p => p.Id == Hero2Id);

                    // Return to the same page with validation message shown
                    return Page();
                }

                // Load full hero data objects from their IDs for further processing
                Hero1 = Products.FirstOrDefault(p => p.Id == Hero1Id);
                Hero2 = Products.FirstOrDefault(p => p.Id == Hero2Id);
                PredictedWinner = Products.FirstOrDefault(p => p.Id == PredictedWinnerId);

                /// <summary>
                /// Determines the actual winner and loser based on hero stats.
                /// Sets the ResultMessage accordingly.
                /// </summary>
                if (Hero1 != null && Hero2 != null)
                {
                    // Calculate base stat totals
                    // Hero 1 Statistics
                    int hero1Stats = Hero1.Intelligence + Hero1.Strength + Hero1.Speed +
                                     Hero1.Durability + Hero1.Power + Hero1.Combat;

                    // Hero 2 statistics
                    int hero2Stats = Hero2.Intelligence + Hero2.Strength + Hero2.Speed +
                                     Hero2.Durability + Hero2.Power + Hero2.Combat;

                    //// Calculate average ratings (default to 0 if null/empty)
                    //// Hero 1 rating
                    //double hero1Rating = (Hero1.Ratings.Length > 0) ? Hero1.Ratings.Average() : 0;

                    //// Hero 2 rating
                    //double hero2Rating = (Hero2.Ratings.Length > 0) ? Hero2.Ratings.Average() : 0;



                    double hero1Rating = GetHeroRating(Hero1);
                    double hero2Rating = GetHeroRating(Hero2);


                    // Combine stats and rating: 90% stats + 10% rating (rating scaled to 50)
                    // Hero 1 total
                    double hero1Total = hero1Stats * 0.9 + hero1Rating * 10;

                    // Hero 2 total
                    double hero2Total = hero2Stats * 0.9 + hero2Rating * 10;

                    // Determine winner and loser
                    bool hero1Wins = hero1Total > hero2Total;
                    ActualWinner = hero1Wins ? Hero1 : Hero2;
                    Loser = hero1Wins ? Hero2 : Hero1;

                    // Evaluate prediction
                    bool predictionCorrect = PredictedWinner.Id == ActualWinner.Id;
                    if (predictionCorrect)
                    {
                        ResultMessage = "You predicted correctly! 🎉";
                    }
                    ResultMessage = "Oops! Your prediction was wrong. 😢";

                }

                Step = BattleStep.ShowResult;
                return Page();
            }

            return Page();
        }

    }
}
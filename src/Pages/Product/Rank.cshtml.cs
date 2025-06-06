using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// PageModel for the Rank page, responsible for providing superhero data sorted by score
    /// </summary>
    public class RankModel : PageModel
    {
        // Injected JSON file product service for accessing superhero data
        private readonly JsonFileProductService ProductService;

        // Injected poll service for reading/writing persistent vote data
        private readonly JsonFilePollService PollService;

        /// <summary>
        /// List of superheroes displayed on the Rank page, sorted by their score
        /// </summary>
        public IEnumerable<ProductModel> Superheroes { get; private set; } = Enumerable.Empty<ProductModel>();

        /// <summary>
        /// Total number of "Yes" poll responses loaded from poll.json
        /// </summary>
        public int YesVotes { get; private set; }

        /// <summary>
        /// Total number of "No" poll responses loaded from poll.json
        /// </summary>
        public int NoVotes { get; private set; }

        /// <summary>
        /// Calculates the percentage of "Yes" votes out of total votes
        /// </summary>
        public int YesPercentage => CalculatePercentage(YesVotes, TotalVotes);

        /// <summary>
        /// Calculates the percentage of "No" votes out of total votes
        /// </summary>
        public int NoPercentage => CalculatePercentage(NoVotes, TotalVotes);

        /// <summary>
        /// Total number of poll votes submitted
        /// </summary>
        private int TotalVotes => YesVotes + NoVotes;

        /// <summary>
        /// Indicates whether at least one vote has been cast
        /// </summary>
        public bool HasVoted => TotalVotes > 0;

        /// <summary>
        /// Calculates the percentage of a specific vote count relative to total votes
        /// </summary>
        /// <param name="count">The number of votes for a specific response</param>
        /// <param name="total">The total number of votes</param>
        /// <returns>Rounded percentage (0–100)</returns>
        private int CalculatePercentage(int count, int total)
        {
            if (total == 0) return 0;
            return (int)System.Math.Round((double)count / total * 100);
        }

        /// <summary>
        /// Binds the user's poll response ("yes" or "no") on form submission
        /// </summary>
        [BindProperty]
        public string? PollResponse { get; set; }

        // Constructor injecting the product and poll services
        public RankModel(JsonFileProductService productService, JsonFilePollService pollService)
        {
            ProductService = productService;
            PollService = pollService;
        }

        /// <summary>
        /// Handles GET requests to the Rank page, fetches and sorts superheroes by score
        /// </summary>
        public void OnGet()
        {
            Superheroes = ProductService.GetProducts().OrderByDescending(p => p.Score);
            LoadPoll();
        }

        /// <summary>
        /// Handles POST requests from the poll form, updates vote counters based on user input,
        /// and reloads the superhero data
        /// </summary>
        /// <returns>Returns the current page with updated vote state</returns>
        public IActionResult OnPost()
        {
            Superheroes = ProductService.GetProducts().OrderByDescending(p => p.Score);

            // Load existing poll results
            var poll = PollService.GetPollResult();

            // Update based on user input
            if (PollResponse == "yes") poll.YesVotes++;
            else if (PollResponse == "no") poll.NoVotes++;

            // Save updated results to JSON file
            PollService.SavePollResult(poll);

            // Update local properties for rendering
            YesVotes = poll.YesVotes;
            NoVotes = poll.NoVotes;

            return Page();
        }

        /// <summary>
        /// Loads existing poll results from JSON file into local properties
        /// </summary>
        private void LoadPoll()
        {
            var poll = PollService.GetPollResult();
            YesVotes = poll.YesVotes;
            NoVotes = poll.NoVotes;
        }
    }
}

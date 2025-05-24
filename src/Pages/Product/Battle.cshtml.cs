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
    /// Enum representing the current step of the battle process.
    /// </summary>
    public enum BattleStep
    {
        SelectHeroes = 1,
        VoteWinner = 2,
        ShowResult = 3
    }

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
            Products = _productService.GetProducts().ToList();

            HeroOptions = Products.Select(p => new SelectListItem
            {
                Value = p.Id,
                Text = p.Title
            }).ToList();

            if (Step == BattleStep.SelectHeroes)
            {
                if (string.IsNullOrEmpty(Hero1Id) || string.IsNullOrEmpty(Hero2Id) || Hero1Id == Hero2Id)
                {
                    ModelState.AddModelError(string.Empty, "Please select two different heroes.");
                    return Page();
                }

                Hero1 = Products.FirstOrDefault(p => p.Id == Hero1Id);
                Hero2 = Products.FirstOrDefault(p => p.Id == Hero2Id);

                Step = BattleStep.VoteWinner;
                return Page();
            }
            else if (Step == BattleStep.VoteWinner)
            {
                if (string.IsNullOrEmpty(PredictedWinnerId))
                {
                    ModelState.AddModelError(string.Empty, "Please select who you think will win.");
                    Hero1 = Products.FirstOrDefault(p => p.Id == Hero1Id);
                    Hero2 = Products.FirstOrDefault(p => p.Id == Hero2Id);
                    return Page();
                }

                Hero1 = Products.FirstOrDefault(p => p.Id == Hero1Id);
                Hero2 = Products.FirstOrDefault(p => p.Id == Hero2Id);
                PredictedWinner = Products.FirstOrDefault(p => p.Id == PredictedWinnerId);

                /// <summary>
                /// Determines the actual winner and loser based on hero stats.
                /// Sets the ResultMessage accordingly.
                /// </summary>
                if (Hero1 != null && Hero2 != null)
                {
                    int hero1Total = Hero1.Intelligence + Hero1.Strength + Hero1.Speed +
                                     Hero1.Durability + Hero1.Power + Hero1.Combat;

                    int hero2Total = Hero2.Intelligence + Hero2.Strength + Hero2.Speed +
                                     Hero2.Durability + Hero2.Power + Hero2.Combat;

                    if (hero1Total > hero2Total)
                    {
                        ActualWinner = Hero1;
                        Loser = Hero2;
                    }
                    else if (hero2Total > hero1Total)
                    {
                        ActualWinner = Hero2;
                        Loser = Hero1;
                    }
                    else
                    {
                        ActualWinner = Hero1;
                        Loser = Hero2;
                    }

                    if (PredictedWinner?.Id == ActualWinner?.Id)
                    {
                        ResultMessage = "You predicted correctly! 🎉";
                    }
                    else
                    {
                        ResultMessage = "Oops! Your prediction was wrong. 😢";
                    }
                }

                Step = BattleStep.ShowResult;
                return Page();
            }

            return Page();
        }
    }
}
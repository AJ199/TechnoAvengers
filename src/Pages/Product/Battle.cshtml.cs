using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public enum BattleStep
    {
        SelectHeroes = 1,
        VoteWinner = 2,
        ShowResult = 3
    }

    public class BattleModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public BattleModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        public List<ProductModel> Products { get; set; } = new();

        [BindProperty]
        public string? Hero1Id { get; set; }

        [BindProperty]
        public string? Hero2Id { get; set; }

        public ProductModel? Hero1 { get; set; }
        public ProductModel? Hero2 { get; set; }

        [BindProperty]
        public string? PredictedWinnerId { get; set; }

        public ProductModel? PredictedWinner { get; set; }

        public ProductModel? ActualWinner { get; set; }
        public ProductModel? Loser { get; set; }
        public string? ResultMessage { get; set; }

        public List<SelectListItem> HeroOptions { get; set; } = new();

        [BindProperty]
        public BattleStep Step { get; set; } = BattleStep.SelectHeroes;

        public void OnGet()
        {
            Products = _productService.GetProducts().ToList();

            HeroOptions = Products.Select(p => new SelectListItem
            {
                Value = p.Id,
                Text = p.Title
            }).ToList();
        }

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
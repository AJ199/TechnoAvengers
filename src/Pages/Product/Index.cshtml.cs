using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class IndexModel : PageModel
    {
        public List<ContosoCrafts.WebSite.Models.ProductModel> Products { get; set; }

        // Add query-bound properties for sorting
        [BindProperty(SupportsGet = true)]
        public string SortField { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        public void OnGet()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");
            var json = System.IO.File.ReadAllText(filePath);
            Products = JsonConvert.DeserializeObject<List<ContosoCrafts.WebSite.Models.ProductModel>>(json);

            // Apply sorting based on query
            if (!string.IsNullOrEmpty(SortField))
            {
                if (SortField == "Title")
                {
                    if (SortOrder == "desc")
                    {
                        Products = Products.OrderByDescending(p => p.Title).ToList();
                    }
                    else
                    {
                        Products = Products.OrderBy(p => p.Title).ToList();
                    }
                }
                // Add more fields here if you want to sort by other properties
            }
        }
    }
}

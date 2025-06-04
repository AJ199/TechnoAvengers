using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Handles displaying and sorting the list of products on the index page.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The list of all products loaded from the JSON file.
        /// </summary>
        public List<ContosoCrafts.WebSite.Models.ProductModel> Products { get; set; }

        /// <summary>
        /// Field to sort the product list by (e.g., "Title").
        /// Bound from query parameters.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SortField { get; set; }

        /// <summary>
        /// Direction of sorting (e.g., "asc" or "desc").
        /// Bound from query parameters.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        /// <summary>
        /// Handles GET requests by loading products from JSON and applying optional sorting.
        /// </summary>
        public void OnGet()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");
            var json = System.IO.File.ReadAllText(filePath);
            Products = JsonConvert.DeserializeObject<List<ContosoCrafts.WebSite.Models.ProductModel>>(json);

            // Apply sorting based on query
            if (!string.IsNullOrEmpty(SortField) && SortField == "Title")
            {
                bool isDescending = SortOrder == "desc";

                Products = isDescending
                    ? Products.OrderByDescending(p => p.Title).ToList()
                    : Products.OrderBy(p => p.Title).ToList();
            }
        }
    }
}


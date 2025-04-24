using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class IndexModel : PageModel
    {
        public List<ContosoCrafts.WebSite.Models.Product> Products { get; set; }

        public void OnGet()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");

            var json = System.IO.File.ReadAllText(filePath);

            Products = JsonConvert.DeserializeObject<List<ContosoCrafts.WebSite.Models.Product>>(json);
        }
    }
}

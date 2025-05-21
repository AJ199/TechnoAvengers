using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Handles the logic for displaying the Privacy page.
    /// </summary>
    public class PrivacyModel : PageModel
    {
        /// <summary>
        /// Logger instance for capturing diagnostics and tracing.
        /// </summary>
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Constructor that initializes the logger.
        /// </summary>
        /// <param name="logger">Logger used for capturing runtime information</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to the Privacy page.
        /// </summary>
        public void OnGet()
        {
            // No logic is required for static content display.
        }
    }
}
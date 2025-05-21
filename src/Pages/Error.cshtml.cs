using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Handles error page logic and displays diagnostic information.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// The request ID used for tracing and debugging.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indicates whether the RequestId should be shown.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Logger instance for logging error details.
        /// </summary>
        private readonly ILogger<ErrorModel> _logger;

        /// <summary>
        /// Constructor that initializes the logger.
        /// </summary>
        /// <param name="logger">Logger used for error logging</param>
        public ErrorModel(ILogger<ErrorModel> logger) => _logger = logger;

        /// <summary>
        /// Handles GET requests and sets the RequestId.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
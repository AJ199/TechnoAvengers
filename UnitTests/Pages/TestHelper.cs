using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

using Microsoft.Extensions.Logging;

using Moq;

using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UnitTests
{
    /// <summary>
    /// Helper to hold the web start settings
    ///
    /// HttpClient
    /// 
    /// Action Contect
    /// 
    /// The View Data and Teamp Data
    /// 
    /// The Product Service
    /// </summary>
    public static class TestHelper
    {
        // Mock logger for capturing log output during tests
        public static Mock<ILogger<IndexModel>> MockLogger;

        // Mocked IWebHostEnvironment to simulate hosting environment properties
        public static Mock<IWebHostEnvironment> MockWebHostEnvironment;

        // Factory used to generate URL helpers in unit tests
        public static IUrlHelperFactory UrlHelperFactory;

        // Default HTTP context object for simulating requests
        public static DefaultHttpContext HttpContextDefault;

        // WebHost environment interface used to access hosting properties
        public static IWebHostEnvironment WebHostEnvironment;

        // Model validation state
        public static ModelStateDictionary ModelState;

        // Combines route, model, and HTTP context
        public static ActionContext ActionContext;

        // Provider for model metadata, required to build ViewData
        public static EmptyModelMetadataProvider ModelMetadataProvider;

        // Dictionary to pass data from controller to view
        public static ViewDataDictionary ViewData;

        // Dictionary for storing temporary data across requests
        public static TempDataDictionary TempData;

        // Context for Razor Page execution
        public static PageContext PageContext;

        // Service to access superhero data from JSON file
        public static JsonFileProductService ProductService;

        // Service to access comment data from JSON file
        public static JsonFileCommentService CommentService;

        // Default IObjectModelValidator implementation for real validation
        public static IObjectModelValidator ObjectModelValidator;

        public static JsonFilePollService PollService;

        /// <summary>
        /// Provides setup and configuration for web-related services during unit tests
        /// </summary>
        static TestHelper()
        {
            // Captures IndexModel logs
            MockLogger = new Mock<ILogger<IndexModel>>();

            // Mock web host environment configuration
            MockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            MockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            MockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(TestFixture.DataWebRootPath);
            MockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(TestFixture.DataContentRootPath);

            // Create and configure the default HTTP context
            HttpContextDefault = new DefaultHttpContext()
            {
                TraceIdentifier = "trace",
            };
            HttpContextDefault.HttpContext.TraceIdentifier = "trace";

            // Initializes a fresh model state dictionary
            ModelState = new ModelStateDictionary();

            // Combine HTTP context, routing data, and model state into an ActionContext
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);

            // Sets up model metadata provider
            ModelMetadataProvider = new EmptyModelMetadataProvider();

            // Sets view data dictionary
            ViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);

            // Sets up temporary data dictionary
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            // Create a page context that includes view data and HTTP context
            PageContext = new PageContext(ActionContext)
            {
                ViewData = ViewData,
                HttpContext = HttpContextDefault
            };

            // JSON product service using the mock environment
            ProductService = new JsonFileProductService(MockWebHostEnvironment.Object);

            CommentService = new JsonFileCommentService(MockWebHostEnvironment.Object);

            PollService = new JsonFilePollService(MockWebHostEnvironment.Object);

        }

        /// <summary>
        /// Loads and binds the EmailSettings section from appsettingsTest.json for unit tests
        /// </summary>
        /// <returns>EmailSettingsModel wrapped in IOptions</returns>
        public static IOptions<EmailSettingsModel> LoadEmailSettings()
        {
            // Build configuration using the test environment base directory
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettingsTests.json", optional: false, reloadOnChange: true)
                .Build();

            // Extract and bind the EmailSettings section
            var settings = config.GetSection("EmailSettings").Get<EmailSettingsModel>();

            // Wrap the settings object in IOptions and return it
            return Options.Create(settings);
        }

        /// <summary>
        /// Configures HttpContext.RequestServices to enable TryValidateModel during Razor Page unit tests
        /// </summary>
        /// <param name="pageModel">The PageModel instance to modify</param>
        public static void EnableValidation(PageModel pageModel)
        {
            // Validator object
            var objectValidator = new Mock<IObjectModelValidator>();

            objectValidator
                .Setup(o => o.Validate(
                    It.IsAny<ActionContext>(),
                    It.IsAny<ValidationStateDictionary>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()));

            // Mock service provider
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IObjectModelValidator)))
                .Returns(objectValidator.Object);

            // Assign both PageContext and ControllerContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider.Object
            };

            pageModel.PageContext = new PageContext
            {
                HttpContext = httpContext,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            };
        }
    }
}
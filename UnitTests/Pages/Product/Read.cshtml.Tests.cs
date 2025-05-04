
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Pages.Product.Read
{
    public class ReadTests
    {
        #region TestSetup
        public static IUrlHelperFactory urlHelperFactory;
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;

        public static ReadModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext()
            {
                //RequestServices = serviceProviderMock.Object,
            };

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net8.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new ReadModel(productService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet

        // OnGet on Read should test functionality of a Read of a specific product
        // Tests should include making sure that Read returns the Product in its payload with a valid ID
        // Tests should include InvalidID's

        // OnPost is not handled.

        /// <summary>
        /// Return a Product with a given Id.
        /// We will create a Record, and then search for it
        /// 
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Product()
        {
            // Arrange
            var validId = "31";

            // Act
            pageModel.OnGet(validId);

            // Assert
            Assert.IsNotNull(pageModel.Product);
            Assert.AreEqual("Ant-Man II", pageModel.Product.Title);
        }

        [Test]
        public void OnGet_InvalidId_Should_Return_Null()
        {
            // Arrange
            var invalidId = "avengers";

            // Act
            pageModel.OnGet(invalidId);

            // Assert
            Assert.IsNull(pageModel.Product, "Product should be null for an invalid ID.");
        }

        [Test]
        public void OnGet_NullId_Should_Return_Null()
        {
            // Act
            pageModel.OnGet(null);

            // Assert
            Assert.IsNull(pageModel.Product, "Product should be null when ID is null.");
        }

        [Test]
        public void OnGet_ValidId_Should_Populate_Product_For_Display()
        {
            // Arrange
            var validId = "31"; // Ensure this exists in your products.json

            // Act
            pageModel.OnGet(validId);

            // Assert
            Assert.IsNotNull(pageModel.Product, "Product should not be null.");
            Assert.IsFalse(string.IsNullOrEmpty(pageModel.Product.Title), "Product title should not be empty.");
            Assert.IsFalse(string.IsNullOrEmpty(pageModel.Product.ImageUrl), "Image URL should not be empty.");
            Assert.IsNotNull(pageModel.Product.Powerstats, "Powerstats should not be null.");
            Assert.IsNotNull(pageModel.Product.Fullname, "Fullname should be populated.");
            Assert.IsNotNull(pageModel.Product.Birthplace, "Birthplace should be populated.");
            Assert.IsNotNull(pageModel.Product.Work, "Work should be populated.");
            Assert.IsNotNull(pageModel.Product.FirstAppear, "First appearance should be populated.");
        }

        #endregion OnGet
    }
}
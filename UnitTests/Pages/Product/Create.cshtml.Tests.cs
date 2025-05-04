using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using ContosoCrafts.WebSite.Services;
using Newtonsoft.Json;

namespace UnitTests.Pages.Product
{
    public class CreateTests
    {
        #region TestSetup
        public static DefaultHttpContext httpContextDefault;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;
        public static CreateModel pageModel;
        public static string testFilePath;

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

            pageModel = new CreateModel();
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        public void OnGet_Should_Return_Page()
        {
            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
        }

        [Test]
        public void OnPost_ValidProduct_Should_Create_And_Redirect()
        {
            // Arrange
            var newProduct = new ProductModel
            {
                Title = "Spider-Man",
                Fullname = "Peter Parker",
                Birthplace = "Queens",
                Work = "movie"
            };

            pageModel.Product = newProduct;

            // Redirect file access to test path (simulate production logic)
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");
            Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));
            File.WriteAllText(jsonPath, "[]");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var redirect = result as RedirectToPageResult;
            Assert.AreEqual("Read", redirect.PageName);
            Assert.IsNotNull(redirect.RouteValues);
            Assert.IsTrue(redirect.RouteValues.ContainsKey("id"));
        }

        [Test]
        public void OnPost_ValidProduct_WithExistingProducts_ShouldAppendAndRedirect()
        {
            // Arrange
            var existingProducts = new List<ProductModel>
    {
        new ProductModel
        {
            Id = "1",
            Title = "Spider-Man",
            Fullname = "Peter Parker",
            Birthplace = "Queens"

        }
    };

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "products.json");
            Directory.CreateDirectory(Path.GetDirectoryName(jsonPath));

            // Write an existing product into the file
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(existingProducts, Formatting.Indented));

            var newProduct = new ProductModel
            {
                Title = "Iron Man",
                Fullname = "Tony Stark",
                Birthplace = "New York",
                Work = "Avenger"
            };

            pageModel.Product = newProduct;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);

            var redirect = result as RedirectToPageResult;
            Assert.AreEqual("Read", redirect.PageName);
            Assert.IsNotNull(redirect.RouteValues);
            Assert.IsTrue(redirect.RouteValues.ContainsKey("id"));

            // Validate new product added correctly
            var finalJson = File.ReadAllText(jsonPath);
            var updatedProducts = JsonConvert.DeserializeObject<List<ProductModel>>(finalJson);

            Assert.AreEqual(2, updatedProducts.Count);
            Assert.AreEqual("Iron Man", updatedProducts.Last().Title);
        }

        #endregion OnGet

    }
}

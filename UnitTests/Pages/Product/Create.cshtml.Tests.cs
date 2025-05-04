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
            httpContextDefault = new DefaultHttpContext();
            modelState = new ModelStateDictionary();
            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);
            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());
            pageContext = new PageContext(actionContext) { ViewData = viewData };

            // Setup a test file for JSON storage
            testFilePath = Path.Combine(Path.GetTempPath(), "test-products.json");
            File.WriteAllText(testFilePath, "[]"); // Start with empty list

            pageModel = new CreateModel
            {
                PageContext = pageContext,
                TempData = tempData
            };

            // Redirect file path inside the OnPost method to our test file using reflection (or abstract to injectable in real app)
        }
        #endregion TestSetup
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
                Title = "Test Product",
                Fullname = "Test",
                Birthplace = "test2",
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

    }
}

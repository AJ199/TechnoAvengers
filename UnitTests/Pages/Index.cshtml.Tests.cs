using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Reflection;
using ContosoCrafts.WebSite.Models;
using System.Threading.Tasks;

namespace UnitTests.Pages
{
    /// <summary>
    /// Tests related to homepage behavior
    /// </summary>
    public class IndexModelTests
    {
        #region Setup
        private Mock<ILogger<IndexModel>> _loggerMock;
        private IndexModel _pageModel;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<IndexModel>>();
            _pageModel = new IndexModel(_loggerMock.Object, TestHelper.ProductService)
            {
                PageContext = TestHelper.PageContext
            };
        }

        #endregion Setup

        #region OnGet
        /// <summary>
        /// Verifies that calling OnGet with no query parameters returns all products.
        /// </summary>
        [Test]
        public void OnGet_Valid_Empty_Query_Returns_All_Heroes()
        {
            // Arrange
            _pageModel.PageContext.HttpContext = new DefaultHttpContext();

            // Act
            _pageModel.OnGet();

            // Assert
            var data = TestHelper.ProductService.GetProducts();
            Assert.AreEqual(data.Count(), _pageModel.FilteredHeroes.Count);

            // Reset
        }

        /// <summary>
        /// Ensures OnGet correctly filters products by search term.
        /// </summary>
        [Test]
        public void OnGet_Valid_Search_Term_Returns_Filtered_By_Title()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?SearchTerm=iron");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes.All(h => h.Title.ToLower().Contains("iron"));
            Assert.AreEqual(true, result);

            // Reset
        }

        /// <summary>
        /// Ensures filtering by alignment returns the correct subset of heroes.
        /// </summary>
        [Test]
        public void OnGet_Valid_Alignment_Good_Returns_Good_Heroes()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?Alignment=good");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes.All(h => h.Alignment == "good");
            Assert.AreEqual(true, result);

            // Reset
        }

        /// <summary>
        /// Ensures filtering by role returns correct heroes.
        /// </summary>
        [Test]
        public void OnGet_Valid_Role_Core_Avenger_Returns_Core_Avengers()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?Role=Core Avenger");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes.All(h => h.Role == "Core Avenger");
            Assert.AreEqual(true, result);

            // Reset
        }

        /// <summary>
        /// Ensures filtering by gender returns correct heroes.
        /// </summary>
        [Test]
        public void OnGet_Valid_Gender_Male_Returns_Male_Heroes()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?Gender=Male");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes.All(h => h.Gender == "Male");
            Assert.AreEqual(true, result);

            // Reset
        }

        /// <summary>
        /// Ensures that all filters are applied simultaneously.
        /// </summary>
        [Test]
        public void OnGet_Valid_Multiple_Filters_Returns_Correct_Subset()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?Alignment=good&Gender=Male&Role=Core Avenger");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes.All(h =>
                h.Alignment == "good" &&
                h.Gender == "Male" &&
                h.Role == "Core Avenger");
            Assert.AreEqual(true, result);

            // Reset
        }

        /// <summary>
        /// Ensures stat filters work within a valid range.
        /// </summary>
        [Test]
        public void OnGet_Valid_Stat_Range_Filter_Returns_Matching_Stats()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?IntelligenceMin=80&IntelligenceMax=100");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes.All(h =>
                h.Intelligence >= 80 && h.Intelligence <= 100);
            Assert.AreEqual(true, result);

            // Reset
        }

        /// <summary>
        /// Verifies that OnGet() filters the hero list when a valid search term is passed through the query
        /// </summary>
        [Test]
        public void OnGet_Valid_Search_Term_Provided_In_Query_String_Should_Return_Filtered_Heroes_Containing_Search_Term()
        {
            // Arrange
            // Use fresh context
            var context = new DefaultHttpContext();
            var request = context.Request;

            // Simulate query string ?SearchTerm=thor
            request.QueryString = new QueryString("?SearchTerm=thor");
            var queryCollection = new QueryCollection(
                new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
                {
                    { "SearchTerm", "thor" }
                }
            );
            context.Request.Query = queryCollection;

            var data = new IndexModel(
                new Mock<ILogger<IndexModel>>().Object,
                TestHelper.ProductService)
            {
                PageContext = new PageContext
                {
                    HttpContext = context
                }
            };

            // Act
            data.OnGet();

            // Assert
            var result = data.FilteredHeroes;
            Assert.AreEqual(true, result.Count > 0);
            Assert.AreEqual(true, result.Any(hero => hero.Title.ToLower().Contains("thor")));

            // Reset
            context.Request.QueryString = QueryString.Empty;
        }

        /// <summary>
        /// Sorts heroes by Title in ascending order.
        /// </summary>
        [Test]
        public void OnGet_Sort_By_Title_Ascending_Returns_Sorted_List()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?SortField=Title&SortOrder=asc");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes;
            var sorted = result.OrderBy(h => h.Title).ToList();
            Assert.IsTrue(result.SequenceEqual(sorted));
        }

        /// <summary>
        /// Sorts heroes by Intelligence in descending order.
        /// </summary>
        [Test]
        public void OnGet_Sort_By_Intelligence_Descending_Returns_Sorted_List()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?SortField=Intelligence&SortOrder=desc");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes;
            var sorted = result.OrderByDescending(h => h.Intelligence).ToList();
            Assert.IsTrue(result.SequenceEqual(sorted));
        }

        /// <summary>
        /// Sorts heroes by Strength in descending order.
        /// </summary>
        [Test]
        public void OnGet_Sort_By_Strength_Descending_Returns_Sorted_List()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?SortField=Strength&SortOrder=desc");
            _pageModel.PageContext.HttpContext = context;

            // Act
            _pageModel.OnGet();

            // Assert
            var result = _pageModel.FilteredHeroes;
            var sorted = result.OrderByDescending(h => h.Strength).ToList();
            Assert.IsTrue(result.SequenceEqual(sorted));
        }

        /// <summary>
        /// Test that the default value of SortOrder is 'asc' and it can be changed.
        /// </summary>
        [Test]
        public void SortOrder_Property_Default_And_Set_Should_Pass()
        {
            // Arrange
            var model = _pageModel; // Replace with your actual class name that contains SortOrder

            // Assert default value
            Assert.AreEqual("asc", model.SortOrder);

            // Act
            model.SortOrder = "desc";

            // Assert new value
            Assert.AreEqual("desc", model.SortOrder);
        }

        /// <summary>
        /// Ensures OnGet does not throw or alter list when an invalid SortField is passed.
        /// Covers: property == null
        /// </summary>
        [Test]
        public void OnGet_Invalid_SortField_Does_Not_Sort()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?SortField=NotARealProperty&SortOrder=asc");
            _pageModel.PageContext.HttpContext = context;

            // Add known initial data
            var initial = TestHelper.ProductService.GetProducts().ToList();
            _pageModel.FilteredHeroes = initial.ToList(); // Clone before sort attempt

            // Act
            _pageModel.OnGet();

            // Assert
            var afterSort = _pageModel.FilteredHeroes.Select(h => h.Title).ToList();
            var beforeSort = initial.Select(h => h.Title).ToList();
            CollectionAssert.AreEqual(beforeSort, afterSort);
        }

        #endregion OnGet

        #region GetValuesForCategory
        /// <summary>
        /// Validates correct values returned for Alignment category.
        /// </summary>
        [Test]
        public void GetValuesForCategory_Valid_Alignment_Returns_Expected_Values()
        {
            // Act
            var result = _pageModel.GetValuesForCategory("Alignment");

            // Assert
            Assert.AreEqual(new List<string> { "good", "bad", "neutral" }, result);

            // Reset
        }

        /// <summary>
        /// Validates correct values returned for Gender category.
        /// </summary>
        [Test]
        public void GetValuesForCategory_Valid_Gender_Returns_Expected_Values()
        {
            // Act
            var result = _pageModel.GetValuesForCategory("Gender");

            // Assert
            Assert.AreEqual(new List<string> { "Male", "Female", "Other" }, result);

            // Reset
        }

        /// <summary>
        /// Validates correct values returned for Role category.
        /// </summary>
        [Test]
        public void GetValuesForCategory_Valid_Role_Returns_Expected_Values()
        {
            // Act
            var result = _pageModel.GetValuesForCategory("Role");

            // Assert
            Assert.AreEqual(new List<string> { "Core Avenger", "Founding Avenger", "Mystic Defender", "Guardian", "Support" }, result);

            // Reset
        }

        /// <summary>
        /// Ensures unknown category returns an empty list.
        /// </summary>
        [Test]
        public void GetValuesForCategory_InValid_Unknown_Category_Returns_EmptyList()
        {
            // Act
            var result = _pageModel.GetValuesForCategory("Unknown");

            // Assert
            Assert.AreEqual(0, result.Count);

            // Reset
        }

        #endregion GetValuesForCategory

        #region GetMin
        /// <summary>
        /// Ensures GetMin returns a valid integer from query.
        /// </summary>
        [Test]
        public void GetMin_Valid_Parsed_Value_From_Query_Returns_Correct_Min()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?StrengthMin=50");
            _pageModel.PageContext.HttpContext = context;

            // Act
            var result = _pageModel.GetMin("Strength");

            // Assert
            Assert.AreEqual(50, result);

            // Reset
        }

        /// <summary>
        /// Ensures GetMin defaults when input is invalid.
        /// </summary>
        [Test]
        public void GetMin_InValid_Query_Returns_Default_Min()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?StrengthMin=invalid");
            _pageModel.PageContext.HttpContext = context;

            // Act
            var result = _pageModel.GetMin("Strength");

            // Assert
            Assert.AreEqual(1, result);

            // Reset
        }

        #endregion GetMin

        #region GetMax
        /// <summary>
        /// Ensures GetMax returns a valid integer from query.
        /// </summary>
        [Test]
        public void GetMax_Valid_Parsed_Value_From_Query_Returns_Correct_Max()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?StrengthMax=75");
            _pageModel.PageContext.HttpContext = context;

            // Act
            var result = _pageModel.GetMax("Strength");

            // Assert
            Assert.AreEqual(75, result);

            // Reset
        }

        /// <summary>
        /// Ensures GetMax defaults when input is invalid.
        /// </summary>
        [Test]
        public void GetMax_Invalid_Query_Returns_Default_Max()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.QueryString = new QueryString("?StrengthMax=invalid");
            _pageModel.PageContext.HttpContext = context;

            // Act
            var result = _pageModel.GetMax("Strength");

            // Assert
            Assert.AreEqual(100, result);

            // Reset
        }

        #endregion GetValuesForCategory

    }
}
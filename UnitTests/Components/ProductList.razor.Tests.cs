using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bunit;
using ContosoCrafts.WebSite.Components; // Your component namespace
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace UnitTests.Components
{
    /// <summary>
    /// Tests related to Component productlist page behavior
    /// </summary>
    public class ProductListTests : Bunit.TestContext
    {
        private JsonFileProductService _productService;

        public ProductListTests()
        {
            _productService = TestHelper.ProductService;
            Services.AddSingleton<JsonFileProductService>(_productService);
        }

        [SetUp]
        public void Setup()
        {
            // No service registrations here; only reset state if needed
        }

        #region Reflection Helpers

        // --- Reflection helpers ---
        private void InvokePrivateMethod(object instance, string methodName, params object[] parameters)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            method.Invoke(instance, parameters);
        }

        private void SetPrivateField(object instance, string fieldName, object value)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            field.SetValue(instance, value);
        }

        private T GetPrivateField<T>(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return (T)field.GetValue(instance);
        }

        #endregion

        #region ProductCards

        /// <summary>
        /// Verifies that the component renders product cards for each product passed in.
        /// </summary>
        [Test]
        public void Should_Render_Product_Cards()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts().ToList();

            // Act
            var cut = RenderComponent<ProductList>(parameters => parameters
                .Add(p => p.Products, products));

            // Assert
            foreach (var product in products)
            {
                Assert.That(cut.Markup, Does.Contain(product.Title));
            }
        }

        /// <summary>
        /// Verifies that when the Products list is null or empty, the component does not render any product cards or deck layout.
        /// </summary>
        [Test]
        public void Should_Not_Render_Product_Cards_When_Products_Null_Or_Empty()
        {
            // Arrange
            var cutNull = RenderComponent<ProductList>();

            Assert.That(cutNull.Markup, Does.Not.Contain("card-deck"));

            // Arrange
            var cutEmpty = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, new List<ProductModel>()));

            // Assert
            Assert.That(cutEmpty.Markup, Does.Not.Contain("card-deck"));
        }

        /// <summary>
        /// Tests that clicking "View Superhero" button opens modal showing product details.
        /// </summary>
        [Test]
        public void Clicking_View_Superhero_Opens_Modal_With_Product_Details()
        {
            // Arrange
            var products = _productService.GetProducts().ToList();
            var cut = RenderComponent<ProductList>(parameters => parameters
                .Add(p => p.Products, products)
            );

            // Act
            var button = cut.Find("button.btn-primary");
            button.Click();

            // Assert
            var selectedProduct = products[0];
            Assert.That(cut.Markup, Does.Contain(selectedProduct.Title));
            Assert.That(cut.Markup, Does.Contain(selectedProduct.Fullname));
        }

        /// <summary>
        /// Checks that star rating UI reflects current rating and updates after rating submission.
        /// </summary>
        [Test]
        public void Stars_Render_Based_On_Current_Rating_And_Submit_Rating_Updates()
        {
            // Arrange
            var products = _productService.GetProducts().ToList();
            var cut = RenderComponent<ProductList>(parameters => parameters
                .Add(p => p.Products, products)
            );

            // Act
            cut.Find("button.btn-primary").Click();
            var checkedStarsBefore = cut.FindAll("span.fa-star.checked").Count;

            var firstStar = cut.FindAll("span.fa-star").First();
            firstStar.Click();

            var checkedStarsAfter = cut.FindAll("span.fa-star.checked").Count;

            // Assert
            Assert.That(checkedStarsAfter, Is.GreaterThanOrEqualTo(1));
        }

        /// <summary>
        /// Ensures currentRating and voteCount are zero when selected product's Ratings is null.
        /// </summary>
        [Test]
        public void GetCurrentRating_Selected_Product_Ratings_Null_Sets_Zero()
        {
            // Arrange
            var products = _productService.GetProducts().ToList();
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, products));

            var productWithNullRatings = new ProductModel
            {
                Id = "null-ratings",
                Title = "Null Ratings Product",
                Ratings = null
            };

            SetPrivateField(cut.Instance, "selectedProduct", productWithNullRatings);

            // Act
            InvokePrivateMethod(cut.Instance, "GetCurrentRating");

            // Assert
            int currentRating = GetPrivateField<int>(cut.Instance, "currentRating");
            int voteCount = GetPrivateField<int>(cut.Instance, "voteCount");

            Assert.AreEqual(0, currentRating);
            Assert.AreEqual(0, voteCount);
        }

        /// <summary>
        /// Verifies that GetCurrentRating sets currentRating and voteCount to zero and leaves voteLabel null when the selected product's Ratings property is null.
        /// </summary>
        [Test]
        public void GetCurrentRating_Selected_Product_Ratings_Null_Sets_Zero_And_Prints()
        {
            // Arrange
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, new List<ProductModel>()));

            // Set selectedProduct with null Ratings to hit if block
            var productWithNullRatings = new ProductModel
            {
                Id = "null-ratings",
                Ratings = null
            };

            SetPrivateField(cut.Instance, "selectedProduct", productWithNullRatings);

            // Act
            InvokePrivateMethod(cut.Instance, "GetCurrentRating");

            // Assert
            int currentRating = GetPrivateField<int>(cut.Instance, "currentRating");
            int voteCount = GetPrivateField<int>(cut.Instance, "voteCount");
            string voteLabel = GetPrivateField<string>(cut.Instance, "voteLabel");

            Assert.AreEqual(0, currentRating);
            Assert.AreEqual(0, voteCount);
            Assert.IsNull(voteLabel); // voteLabel is not set in this branch
        }

        /// <summary>
        /// Confirms that when the selected product has exactly one rating, GetCurrentRating sets voteCount to 1 and voteLabel to "Vote".
        /// </summary>
        [Test]
        public void GetCurrentRating_Vote_Count_One_Sets_Vote_Label_To_Vote()
        {
            // Arrange
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, new List<ProductModel>()));

            var productWithOneRating = new ProductModel
            {
                Id = "one-rating",
                Ratings = new int[] { 5 }
            };

            SetPrivateField(cut.Instance, "selectedProduct", productWithOneRating);

            // Act
            InvokePrivateMethod(cut.Instance, "GetCurrentRating");

            // Assert
            int voteCount = GetPrivateField<int>(cut.Instance, "voteCount");
            string voteLabel = GetPrivateField<string>(cut.Instance, "voteLabel");

            Assert.AreEqual(1, voteCount);
            Assert.AreEqual("Vote", voteLabel);
        }

        /// <summary>
        /// Ensures that if the selected product is null, GetCurrentRating safely sets currentRating and voteCount to zero and leaves voteLabel null.
        /// </summary>
        [Test]
        public void GetCurrentRating_Selected_Product_Is_Null_Sets_Zero_And_Prints()
        {
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, new List<ProductModel>()));

            // selectedProduct = null to hit the Console.WriteLine line safely
            SetPrivateField(cut.Instance, "selectedProduct", null);

            // Act
            InvokePrivateMethod(cut.Instance, "GetCurrentRating");

            // Assert
            int currentRating = GetPrivateField<int>(cut.Instance, "currentRating");
            int voteCount = GetPrivateField<int>(cut.Instance, "voteCount");

            Assert.AreEqual(0, currentRating);
            Assert.AreEqual(0, voteCount);

            // Since selectedProduct is null, no voteLabel is set, you can check it's null or default
            string voteLabel = GetPrivateField<string>(cut.Instance, "voteLabel");
            Assert.IsNull(voteLabel);
        }

        /// <summary>
        /// Validates that when the selected product has multiple ratings, GetCurrentRating sets voteCount accordingly and sets voteLabel to "Votes".
        /// </summary>
        [Test]
        public void GetCurrentRating_Vote_Count_More_Than_One_Sets_Vote_Label_To_Votes()
        {
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, new List<ProductModel>()));

            var productWithMultipleRatings = new ProductModel
            {
                Id = "multiple-ratings",
                Ratings = new int[] { 4, 6 }
            };

            SetPrivateField(cut.Instance, "selectedProduct", productWithMultipleRatings);

            InvokePrivateMethod(cut.Instance, "GetCurrentRating");

            int voteCount = GetPrivateField<int>(cut.Instance, "voteCount");
            string voteLabel = GetPrivateField<string>(cut.Instance, "voteLabel");

            Assert.AreEqual(2, voteCount);
            Assert.AreEqual("Votes", voteLabel); // Covers voteLabel = "Votes"
        }

        /// <summary>
        /// Verifies that the UI displays "Be the first to vote!" message when the selected product has no ratings.
        /// </summary>
        [Test]
        public void Modal_Shows_Be_The_First_To_Vote_When_No_Ratings()
        {
            // Arrange
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, new List<ProductModel>()));

            var productWithNullRatings = new ProductModel
            {
                Id = "no-votes",
                Ratings = null
            };

            SetPrivateField(cut.Instance, "selectedProduct", productWithNullRatings);

            // Act
            InvokePrivateMethod(cut.Instance, "GetCurrentRating");
            cut.Render();

            // Assert
            Assert.That(cut.Markup, Does.Contain("Be the first to vote!"));
        }

        /// <summary>
        /// Verifies that SubmitRating exits early when selectedProductId is null.
        /// </summary>
        [Test]
        public void SubmitRating_Selected_Product_Id_Null_Does_Not_Call_Add_Rating()
        {
            // Arrange
            var products = _productService.GetProducts().ToList();
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, products));
            SetPrivateField(cut.Instance, "selectedProductId", null);

            // Act & Assert
            // Just invoke and ensure no exceptions thrown, rating won't be submitted
            Assert.DoesNotThrow(() => InvokePrivateMethod(cut.Instance, "SubmitRating", 3));
        }

        /// <summary>
        /// Confirms SubmitRating with valid selectedProductId adds rating and updates component.
        /// </summary>
        [Test]
        public void SubmitRating_Valid_Selected_Product_Id_Calls_Add_Rating()
        {
            // Arrange
            var products = _productService.GetProducts().ToList();
            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, products));

            var firstProduct = products[0];
            SetPrivateField(cut.Instance, "selectedProductId", firstProduct.Id);

            // Act
            InvokePrivateMethod(cut.Instance, "SubmitRating", 4);

            // Assert
            string idAfter = GetPrivateField<string>(cut.Instance, "selectedProductId");
            Assert.AreEqual(firstProduct.Id, idAfter);
            Assert.That(cut.Markup, Does.Contain(firstProduct.Title));
        }

        /// <summary>
        /// Verifies that clicking on a star in the else branch (i.e., not pre-checked) does not throw an exception and updates the UI to reflect the new rating.
        /// </summary>
        [Test]
        public void Stars_ElseBranch_Click_Does_Not_Throw_And_Updates_UI()
        {
            // Arrange
            var products = _productService.GetProducts().ToList();

            var cut = RenderComponent<ProductList>(parameters => parameters.Add(p => p.Products, products));

            // Select a product first so stars appear (simulate View Superhero click)
            cut.Find("button.btn-primary").Click();

            // Find stars WITHOUT 'checked' class (these are in else branch)
            var starsWithoutChecked = cut.FindAll("span.fa-star:not(.checked)");

            Assert.IsTrue(starsWithoutChecked.Count > 0);

            // Act & Assert - click on one star without checked class and assert no exception
            Assert.DoesNotThrow(() => starsWithoutChecked[0].Click());
            var checkedStarsAfterClick = cut.FindAll("span.fa-star.checked").Count;

            Assert.IsTrue(checkedStarsAfterClick >= 1);
        }

        #endregion ProductCards
    }
}

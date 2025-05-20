using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using ContosoCrafts.WebSite;
using NUnit.Framework;
using Microsoft.Extensions.Hosting;
using ContosoCrafts.WebSite.Models;
using System.Text.Json;

namespace UnitTests.Pages.Startup
{
    /// <summary>
    /// StartupTests class necessary for unit tests
    /// </summary>
    public class StartupTests
    {
        #region TestSetup

        /// <summary>
        /// TestInitialize will initialize Tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        /// <summary>
        /// Startup class
        /// </summary>
        public class Startup : ContosoCrafts.WebSite.Startup
        {
            public Startup(IConfiguration config) : base(config) { }
        }
        #endregion TestSetup

        #region ConfigureServices
        /// <summary>
        /// Will configure tests
        /// </summary>
        [Test]
        public void Startup_Configure_Services_Valid_Defaut_Should_Pass()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.IsNotNull(webHost);
        }
        #endregion ConfigureServices

        #region Configure
        /// <summary>
        /// Will test for Startup Configuration
        /// </summary>
        [Test]
        public void Startup_Configure_Valid_Defaut_Should_Pass()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.IsNotNull(webHost);
        }

        /// <summary>
        /// Test CreateHostBuilder returns a valid IHostBuilder
        /// </summary>
        [Test]
        public void CreateHostBuilder_Valid_Args_Returns_HostBuilder()
        {
            // Arrange
            var args = new string[] { };

            // Act
            var hostBuilder = Program.CreateHostBuilder(args);

            // Assert
            Assert.IsNotNull(hostBuilder);
            Assert.IsInstanceOf<IHostBuilder>(hostBuilder);
        }

        /// <summary>
        /// Test that the Configuration property getter returns the same IConfiguration instance provided to the Startup constructor.
        /// </summary>
        [Test]
        public void ConfigurationProperty_Getter_Returns_Instance()
        {
            // Arrange
            var config = new ConfigurationBuilder().Build();
            var startup = new Startup(config);

            // Act
            var result = startup.Configuration;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(config, result); // Check the same instance is returned
        }

        /// <summary>
        /// Test that the overridden ToString method on ProductModel returns the correct JSON serialized string.
        /// </summary>
        [Test]
        public void ToString_Valid_Product_Model_Returns_Serialized_Json()
        {
            // Arrange
            var product = new ProductModel
            {
                Id = "test123",
                Title = "Tester hero",
                Fullname = "Tester name",
                Birthplace = "-",
                Work = "Tester",
                FirstAppear = "Movie",
                ImageUrl = "https://www.test.com/test/test.jpg",
                Intelligence = 56,
                Strength = 32,
                Speed = 35,
                Durability = 65,
                Power = 60,
                Combat = 84,
                Ratings = null
            };

            var expectedJson = JsonSerializer.Serialize(product);

            // Act
            var result = product.ToString();

            // Assert
            Assert.AreEqual(expectedJson, result);
        }

        #endregion Configure
    }
}
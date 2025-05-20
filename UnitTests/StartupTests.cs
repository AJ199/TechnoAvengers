using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using ContosoCrafts.WebSite;
using NUnit.Framework;
using Microsoft.Extensions.Hosting;

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
        public void Startup_ConfigureServices_Valid_Defaut_Should_Pass()
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
        public void CreateHostBuilder_ValidArgs_ReturnsHostBuilder()
        {
            // Arrange
            var args = new string[] { };

            // Act
            var hostBuilder = Program.CreateHostBuilder(args);

            // Assert
            Assert.IsNotNull(hostBuilder);
            Assert.IsInstanceOf<IHostBuilder>(hostBuilder);
        }

        [Test]
        public void ConfigurationProperty_Getter_ReturnsInstance()
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

        #endregion Configure
    }
}
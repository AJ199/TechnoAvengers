using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ContosoCrafts.WebSite
{
    /// <summary>
    /// Entry point of the web application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method that runs the application.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        /// <summary>
        /// Configures and creates the web host builder with default settings.
        /// </summary>
        /// <param name="args">Command-line arguments used to configure the host.</param>
        /// <returns>An IHostBuilder configured to use Startup.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

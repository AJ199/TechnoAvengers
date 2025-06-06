using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContosoCrafts.WebSite
{
    /// <summary>
    /// Configures application services and middleware
    /// </summary>
    public class Startup
    {
        // Holds the application's configuration values
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor that receives and stores the application's configuration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Registers services for dependency injection
        /// </summary>
        /// <param name="services">Services for the application</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient();
            services.AddControllers();
            services.AddTransient<JsonFileProductService>();
            services.Configure<EmailSettingsModel>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<EmailService>();
            services.AddTransient<JsonFileCommentService>();
            services.AddTransient<JsonFilePollService>();
        }

        /// <summary>
        /// Configures the middleware pipeline for handling HTTP requests
        /// </summary>
        /// <param name="app">Application builder </param>
        /// <param name="env">Provides environment information</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
            });
        }
    }
}
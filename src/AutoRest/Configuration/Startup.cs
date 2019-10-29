using AutoRest.Adapters;
using AutoRest.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoRest.Configuration
{
    /// <summary>
    ///     Startup configuration class.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///     The configuration object.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Configures the services.
        /// </summary>
        /// <param name="services">Services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddControllers();
            services.AddScoped<IDbConnection>(x =>
                new ConnectionObject(Configuration.GetConnectionString("DatabaseContext")));

            // Change to specific database server adapter if you're not using SQL Server
            services.AddScoped<IDbAdapter, SqlServerDbAdapter>();
        }

        /// <summary>
        ///     Configures the app and environment.
        /// </summary>
        /// <param name="app">The app</param>
        /// <param name="env">The environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(routes => { routes.MapControllerRoute("default", "api/{controller}/{action}/"); });
        }
    }
}
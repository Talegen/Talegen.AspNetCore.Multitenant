namespace MultiTenantWebApp
{
    using Common;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Talegen.AspNetCore.Multitenant;
    using Talegen.AspNetCore.Multitenant.Stores;
    using Talegen.AspNetCore.Multitenant.Strategies;

    /// <summary>
    /// This class contains our initial startup routines.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // add a distributed memory cache to handle the example memory source
            services.AddDistributedMemoryCache();

            // add the multi-tenant capabilities. Using the rout strategy, using a memory store, and an example memory source backer.
            services.AddMultiTenancy<SimpleTenant>()
                .WithResolutionStrategy<TenantRouteResolverStrategy>()
                .WithStore<TenantMemoryStore<SimpleTenant>>()
                .WithSource<ExampleMemorySource<SimpleTenant>>();

            // add some sample tenants to source. Just for example, to populate a source backer.
            TenantExamples.AddSampleTenantsToSource<SimpleTenant>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="logger">The logger.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // use the multitenancy middleware
            app.UseMultiTenancy<SimpleTenant>(logger);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // We need ASP.net Core to handle tenant routes too. Our tenant identifier will be the first slug.
                endpoints.MapControllerRoute(
                    name: "tenantRoute",
                    pattern: "/{tenant}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
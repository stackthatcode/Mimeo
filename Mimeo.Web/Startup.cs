using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mimeo.Middle;
using Mimeo.Middle.Config;
using Mimeo.Middle.Identity;


namespace Mimeo.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // *** Old parameter - IConfiguration configuration
        public Startup(IWebHostEnvironment env)
        {
            // In ASP.NET Core 3.0 `env` will be an IWebHostEnvironment, not IHostingEnvironment
            //
            this.Configuration = BuildConfiguration();
        }

        public static IConfigurationRoot BuildConfiguration(IWebHostEnvironment env = null)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env != null)
            {
                builder
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            }

            return builder.Build();
        }


        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the ConfigureContainer method, below.
        public void ConfigureServices(IServiceCollection services)
        {
            // This exists for the sake of enabling the EF Migrations - otherwise, the plan is to rely on
            // ... Autofac to build DBContexts
            //
            services
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Hangfire wire-up
            //
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));


            // Add services to the collection. Don't build or return
            // any IServiceProvider or the ConfigureContainer method
            // won't get called.
            //
            services.AddOptions();

            // Should technically be able to view the services collection and see the impact of this call
            //
            services.AddMvc();

            // This call injects the configuration for our Identity's behaviors
            //
            services.AddMimeoIdentity();
        }
        

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        //
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Read the JSON configuration into Mimeo App Settings
            //
            var appSettings = MimeoAppSettings.ReadConfig(this.Configuration);
            MiddleAutofac.BuildAppSettings(builder, appSettings);
            MiddleAutofac.Build(builder, Configuration);
            MiddleAutofac.BuildIdentityRegistrations(builder, Configuration);
        }


        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        //
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            //
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            // Autofac's documentation is giving bad advice; can't do this without triggering an error
            // ... app.UseMvc(); // NO NO NO!!!
            //
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseExceptionHandler("/Error/Dispatch/500");
                app.UseStatusCodePagesWithReExecute("/Error/Dispatch/{0}");

                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Dispatch/500");
                app.UseStatusCodePagesWithReExecute("/Error/Dispatch/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //
                // app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting(); 

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHangfireDashboard("/hangfire",
                new DashboardOptions
                {
                    Authorization = new[] { new HangfireAuthorizationFilter(SecurityConfig.AdminRole), },
                });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // This invocation enables anything that leverages RazorPages e.g. Default Identity
                // ... or any of the scaffolded and customized Identity pages
                //
                endpoints.MapRazorPages();
            });
        }
    }
}


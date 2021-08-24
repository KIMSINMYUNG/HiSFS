using HiSFS.Api.Host.Data;
using HiSFS.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Syncfusion.Blazor;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

//using RazorComponentsPreview;

namespace HiSFS.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(Options => Options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("ko")
                };

                options.DefaultRequestCulture = new RequestCulture("ko");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider()
                };
            });
            services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(HiSFSLocalizer));

            services.AddSingleton<CacheService>();
            services.AddSingleton<RemoteService>();
            services.AddSingleton<GlobalMessageService>();
            services.AddSingleton<StorageService>();

            services.AddScoped<MessageService>();
            services.AddScoped<Context>();

            services.AddRazorPages();
            //services.AddServerSideBlazor();
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            //services.AddSingleton<WeatherForecastService>();

            services.AddSyncfusionBlazor();

            services.AddProtectedBrowserStorage();

            if (Program.DEBUG == true)
            {


                services.AddDbContext<ApiDbDZICUBEContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DZICUBEContext")));

                services.AddDbContext<ApiDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("HiSFSContext" +
                    "")));



                //services.AddRazorComponentsRuntimeCompilation();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzI2MDgyQDMxMzgyZTMzMmUzMEdMSk9iYUk5NjQxTFFDZkZCMXNvTTJBWGRGajJNTTFtaUsyeTJmUmxZZjg9");

            app.UseRequestLocalization();

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

            if (Program.DEBUG == true)
            {
                //app.UseRazorComponentsRuntimeCompilation();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

    public class HiSFSLocalizer : ISyncfusionStringLocalizer
    {
        public ResourceManager ResourceManager => Resources.SfResources.ResourceManager;

        public string GetText(string key)
        {
            return this.ResourceManager.GetString(key);
        }
    }
}
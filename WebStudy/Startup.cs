namespace WebStudy
{
    using DevExpress.AspNetCore;
    using DevExpress.AspNetCore.Reporting;
    using DevExpress.DashboardAspNetCore;
    using DevExpress.DashboardWeb;
    using Helpers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using WebStudy.Data;
    using WebStudy.Data.Entities;
    using WebStudy.Data.Repositories;
    using WebStudy.Services;


    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public IFileProvider FileProvider { get; }
        public IConfiguration Configuration { get; }

        public IConfigurationSection GetConnectionStrings()
        {
            var connectionStrings = new Dictionary<string, string>
            {
                [$"WebStydy"] = "XpoProvider=Postgres;Server=localhost;User ID=postgres;Password=Admin123;Database=WebStudy;Encoding=UNICODE",
            };
            return new ConfigurationBuilder()
              .SetBasePath(hostingEnvironment.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
              .AddInMemoryCollection(connectionStrings)
              .Build()
              .GetSection("ConnectionStrings");
        }

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
            loggerFactory.AddFile(Configuration.GetSection("Logging"));

        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDevExpressControls();

            services.AddMvc()
                 .AddJsonOptions(
                      options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.AddMvc().AddDefaultDashboardController(configurator =>
               {
                   configurator.SetDashboardStorage(new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath));
                   configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));
               });


            services.ConfigureReportingServices((builder) =>
            {
                builder.ConfigureReportDesigner(designer =>
                {
                    designer.RegisterDataSourceWizardConfigurationConnectionStringsProvider(GetConnectionStrings());
                });
            });


            services.AddDevExpressControls(settings => settings.Resources = ResourcesType.ThirdParty | ResourcesType.DevExtreme);

        
            #region Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(
                 LanguageViewLocationExpanderFormat.Suffix,
                 opts => { opts.ResourcesPath = "Resources"; })
                 .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("es"),
                        new CultureInfo("fr"),
                        new CultureInfo("it"),
                        new CultureInfo("pt"),
                        new CultureInfo("en"),
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have Localized.
                    opts.SupportedUICultures = supportedCultures;
                });
            #endregion

            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = true;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
            })
                      .AddDefaultTokenProviders()
                      .AddEntityFrameworkStores<DataContext>();


            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseNpgsql(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<SeedDb>();

            services.AddScoped<IMailHelper, MailHelper>();

            services.AddScoped<IUserTypeRepository, UserTypeRepository>();

            services.AddScoped<IUserHelper, UserHelper>();

            services.AddScoped<ICountryRepository, CountryRepository>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseStaticFiles();

            app.UseDevExpressControls();


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
                RequestPath = "/node_modules"
            });


            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorage());


            #region Localization
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapDashboardRoute("api/dashboard");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

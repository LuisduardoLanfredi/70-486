using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using exam70486.Filters;
using exam70486.HostedServices;
using exam70486.Jwt;
using exam70486.Middlewares;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace exam70486
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHostedService, CreateFilePerSecond>();

            #region file provider

            IFileProvider phisycalFileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            IFileProvider embededFileProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly());
            IFileProvider compositeFileProvider = new CompositeFileProvider(phisycalFileProvider, embededFileProvider);

            services.AddSingleton(compositeFileProvider);

            #endregion file provider


            var a = Configuration["TokenProperties"]; // -> null
            var b = Configuration["TokenProperties:Audience"];

            services.AddRouting();

            #region authentication

            services.AddOptions();
            services.Configure<TokenProperties>(Configuration.GetSection("TokenProperties"));

            var tokenProperties = new TokenProperties();
            Configuration.GetSection("TokenProperties").Bind(tokenProperties);
            services.AddSingleton(t => tokenProperties);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = tokenProperties.Issuer,

                         ValidateAudience = true,
                         ValidAudience = tokenProperties.Audience,

                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,

                         IssuerSigningKey = JwtSecurityKey.Create(tokenProperties.Key)
                     };

                     options.Events = new JwtBearerEvents
                     {
                         OnAuthenticationFailed = context =>
                         {
                             Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                             return Task.CompletedTask;
                         },
                         OnTokenValidated = context =>
                         {
                             Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                             return Task.CompletedTask;
                         }
                     };
                 });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Member", policy => policy.RequireClaim("MembershipId", "123"));
                options.AddPolicy("Administrator", policy => policy.RequireClaim("AdministratorId"));
            });

            #endregion authentication

            #region log

            services.AddLogging(configure =>
            {
                //configure.add
                configure.AddConsole(opt => opt.IncludeScopes = true);
            });

            #endregion log

            #region cache

            services.AddResponseCaching(opt =>
            {
                opt.SizeLimit = 100;
            });

            #endregion cache

            services.AddDirectoryBrowser();

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile()
                {
                    Duration = 1,
                    Location = ResponseCacheLocation.None
                });
                options.CacheProfiles.Add("mycache", new CacheProfile()
                {
                    Duration = 5,
                    Location = ResponseCacheLocation.Any
                });

                options.Filters.Add(new SampleAuthorizationFilterAsync());
                options.Filters.Add(new SampleResourceFilterAsync());
                options.Filters.Add(new SampleActionFilterAsync());
                options.Filters.Add(new SampleResultFilterAsync());
            }).AddSessionStateTempDataProvider();

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddDistributedMemoryCache();

            services.AddTransient<MyMiddleware>();

            services.AddSingleton<IDependencyInjectionTester, DependencyInjectionTester>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            app.UseSession();

            //app.UseRewriter(
            //    new RewriteOptions()
            //        .AddRedirect("redirect-rule/(.*)", "redirected/$1")
            //        .AddRewrite("rewrite-rule/(.*)", "routetest/get/$1", false)
            //);

            app.UseMvc(r =>
            {
                r.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id}");//,
                                                            //defaults: new { controller = "routetest", action = "get", id = 10 });

                r.MapRoute(
                    name: "test",
                    template: "{controller}/{action}");//,

            });


            //app.UseFileServer(enableDirectoryBrowsing: true);
            //app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = new PathString("/files")
            //});

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = new PathString("/files")
            //});

            #region middleware test

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("Name", "Lanfredi");
            //    await next.Invoke();
            //});

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("AAAAAAAA");
            //});

            #endregion middleware test

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseStatusCodePages(async opt => {
            //    opt.HttpContext.Response.ContentType = "text/plain";
            //    await opt.HttpContext.Response.WriteAsync("Error");
            //});

            //app.UseStatusCodePagesWithRedirects("/error");
            //app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");            

            //app.UseMyMiddleware();
            //app.UseMyStandartMiddleware();

            //app.UseRouter(routes =>
            //    routes.MapRoute(
            //        name: "default_route",
            //        template: "{controller=Home}/{action=Index}/{id?}"));

            //app.UseResponseCaching();

            var supportedCultures = new List<CultureInfo>() {
                new CultureInfo("en-US"),
                new CultureInfo("fi-FI")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });



        }
    }
}

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy;
using Nancy.Owin;

namespace NETCore_Angular1
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
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "api1";
                });

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>//TODO: Doesn't work
            //    {
            //        options.SlidingExpiration = true;
            //        options.Cookie.SameSite = SameSiteMode.Strict;
            //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                    context.Response.Headers.Add("X-Xss-Protection", "1");
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' data:");
                    context.Response.Headers.Remove("X-Powered-By");//TODO: Doesn't work
                    context.Response.Headers.Remove("Server");//TODO: Doesn't work
                    await next();
                });
                app.UseCookiePolicy(new CookiePolicyOptions//TODO: Doesn't work
                {
                    MinimumSameSitePolicy = SameSiteMode.Strict,
                    Secure = CookieSecurePolicy.Always
                });
                app.UseHsts();
                //TODO Refactor the above into custom security middleware
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            //app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            //app.UseOwin(x => x.UseNancy(options =>
            //{
            //    options.Bootstrapper = new Bootstrapper(env);
            //    options.PassThroughWhenStatusCodesAre(HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
            //}));
            app.UseMvc();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}

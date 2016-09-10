using System;
using AutoMapper;
using DotCom.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.Services;
using RestSharp.Authenticators;

namespace DotCom.AppStart
{
    public static class OwnAptStartup
    {
        #region Private Fields

        private static IConfigurationRoot Configuration;

        #endregion Private Fields

        #region Public Methods

        public static IMapper BuildMapper()
        {
            return new MapperConfiguration(cfg =>
            {
            }).CreateMapper();
        }

        public static void ConfigureOwnAptBuild(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public static void UseOwnAptConfiguration(this IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<OpenIdConnectOptions> oidcOptions)
        {
            ConfigureLogging(loggerFactory);
            ConfigureExceptionHandling(app, env);

            UseStaticFiles(app);
            UseCookieAuthentication(app);
            UseOpenIdConnectAuthentication(app, oidcOptions);
            UseMvc(app);
        }

        public static void UseOwnAptServices(this IServiceCollection services)
        {
            AddCookieAuthentication(services);
            AddOpenIdOptions(services);
            AddMvc(services);
            AddOptions(services);
            AddAuth0(services);
            AddAutoMapper(services);
            AddServices(services);
        }

        private static void AddAuth0(IServiceCollection services)
        {
            services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));
        }

        private static void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
        }

        #endregion Public Methods

        #region Private Methods

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddSingleton<IMapper>(BuildMapper());
        }

        private static void AddCookieAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static void AddMvc(IServiceCollection services)
        {
            services.AddMvc();
        }

        private static void AddOpenIdOptions(IServiceCollection services)
        {
            services.Configure<OpenIdConnectOptions>(options =>
            {
                options.AuthenticationScheme = "Auth0";
                options.Authority = $"https://{Configuration["auth0:domain"]}";
                options.ClientId = Configuration["auth0:clientId"];
                options.ClientSecret = Configuration["auth0:clientSecret"];
                options.AutomaticAuthenticate = false;
                options.AutomaticChallenge = false;
                options.ResponseType = "code";
                options.CallbackPath = new PathString("/Account/SignIn");
                options.ClaimsIssuer = "Auth0";
            });
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IEmailService, ContactFormEmailService>();
            services.AddSingleton<IMailGunRestClient>(BuildMailGunRestClient());
        }

        private static IMailGunRestClient BuildMailGunRestClient()
        {
            return new MailGunRestClient("https://api.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator("api", "key-9d43cbe044c65948c59629ea2f280647")
            };
        }

        private static void ConfigureExceptionHandling(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
        }

        private static void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
        }

        private static void UseCookieAuthentication(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
        }

        private static void UseMvc(this IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void UseOpenIdConnectAuthentication(IApplicationBuilder app, IOptions<OpenIdConnectOptions> oidcOptions)
        {
            app.UseOpenIdConnectAuthentication(oidcOptions.Value);
        }

        private static void UseStaticFiles(IApplicationBuilder app)
        {
            app.UseStaticFiles();
        }

        #endregion Private Methods
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Mapping;
using OwnApt.DotCom.Presentation.Service;
using OwnApt.DotCom.Settings;
using OwnApt.RestfulProxy.Client;
using OwnApt.RestfulProxy.Interface;
using RestSharp.Authenticators;
using Serilog;

namespace OwnApt.DotCom.AppStart
{
    public static class OwnAptStartup
    {
        #region Private Fields

        private static IConfigurationRoot Configuration;

        private static IHostingEnvironment HostEnvironment;

        #endregion Private Fields

        #region Public Methods

        public static IMapper BuildAutoMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AccountProfile>();
                cfg.AddProfile<OwnerProfile>();
            }).CreateMapper();
        }

        public static void ConfigureOwnAptStartup(IConfigurationRoot configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public static void UseOwnAptConfiguration(this IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime, IOptions<OpenIdConnectOptions> oidcOptions)
        {
            ConfigureLogging(loggerFactory, appLifetime);
            ConfigureExceptionHandling(app);

            UseStaticFiles(app);
            UseCookieAuthentication(app);
            UseOpenIdConnectAuthentication(app, oidcOptions);
            UseMvc(app);
            UseCors(app);
        }

        public static void UseOwnAptServices(this IServiceCollection services)
        {
            AddCookieAuthentication(services);
            AddOpenIdOptions(services);
            AddMvc(services);
            AddOptions(services);
            AddServiceUris(services);
            AddAuth0(services);
            AddCores(services);
            AddAutoMapper(services);
            AddServices(services);
            AddRestfulProxy(services);
            AddFeatureToggles(services);
        }

        #endregion Public Methods

        #region Private Methods

        private static void AddAuth0(IServiceCollection services)
        {
            services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddSingleton(BuildAutoMapper());
        }

        private static void AddCookieAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static void AddCores(IServiceCollection services)
        {
            services.AddCors();
        }

        private static void AddFeatureToggles(IServiceCollection services)
        {
            services.Configure<FeatureToggles>(Configuration.GetSection("FeatureToggles"));
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

        private static void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
        }

        private static void AddRestfulProxy(IServiceCollection services)
        {
            var appId = Configuration["HmacCredentials:AppId"];
            var secretKey = Configuration["HmacCredentials:SecretKey"];
            var proxyConfiguration = new ProxyConfiguration(appId, secretKey);
            var proxy = new Proxy(proxyConfiguration);

            services.AddSingleton<IProxy>(proxy);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IClaimsService, ClaimsService>();
            services.AddTransient<ISignUpService, SignUpService>();
            services.AddTransient<IContactFormService, ContactFormService>();
            services.AddTransient<IAccountPresentationService, AccountPresentationService>();
            services.AddTransient<IOwnerPresentationService, OwnerPresentationService>();
            services.AddTransient<IAccountDomainService, AccountDomainService>();
            services.AddTransient<IOwnerDomainService, OwnerDomainService>();

            services.AddSingleton(BuildMailGunRestClient());
        }

        private static void AddServiceUris(IServiceCollection services)
        {
            services.Configure<ServiceUris>(Configuration.GetSection("ServiceUris"));
        }

        private static IMailGunRestClient BuildMailGunRestClient()
        {
            return new MailGunRestClient("https://api.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator("api", "key-9d43cbe044c65948c59629ea2f280647")
            };
        }

        private static void ConfigureExceptionHandling(IApplicationBuilder app)
        {
            if (HostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
        }

        private static void ConfigureLogging(ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            var logentriesToken = Configuration["Logging:LogentriesToken"];
            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext();

            if (HostEnvironment.IsDevelopment())
            {
                loggerConfig
                    .MinimumLevel.Debug()
                    .WriteTo.Async(a => a.RollingFile("logs\\DotCom-{Date}.txt"));
            }
            else
            {
                loggerConfig
                    .MinimumLevel.Warning()
                    .WriteTo.Async(a => a.Logentries(logentriesToken))
                    .WriteTo.Async(a => a.Console());
            }

            Log.Logger = loggerConfig.CreateLogger();

            loggerFactory.AddSerilog();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }

        private static void UseCookieAuthentication(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
        }

        private static void UseCors(IApplicationBuilder app)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
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

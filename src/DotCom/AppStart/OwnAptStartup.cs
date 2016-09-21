using AutoMapper;
using OwnApt.DotCom.Presentation.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.RestfulProxy.Client;
using OwnApt.RestfulProxy.Interface;
using RestSharp.Authenticators;
using Serilog;
using Serilog.Events;

namespace OwnApt.DotCom.AppStart
{
    public static class OwnAptStartup
    {
        #region Fields

        private static IConfigurationRoot Configuration;

        #endregion Fields

        #region Properties

        public static IHostingEnvironment HostEnvironment { get; private set; }

        #endregion Properties

        #region Methods

        public static IMapper BuildMapper()
        {
            return new MapperConfiguration(cfg =>
            {
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
        }

        public static void UseOwnAptServices(this IServiceCollection services)
        {
            AddCookieAuthentication(services);
            AddOpenIdOptions(services);
            AddMvc(services);
            AddOptions(services);
            AddServiceUris(services);
            AddAuth0(services);
            AddAutoMapper(services);
            AddServices(services);
            AddRestfulProxy(services);
        }

        private static void AddAuth0(IServiceCollection services)
        {
            services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));
        }

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

            services.AddSingleton<IMailGunRestClient>(BuildMailGunRestClient());
        }

        private static void AddServiceUris(IServiceCollection services)
        {
            services.Configure<ServiceUriSettings>(Configuration.GetSection("ServiceUris"));
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
            /* Serilog Configuration */
            var logentriesToken = Configuration["Logging:LogentriesToken"];
            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext();

            if (HostEnvironment.IsDevelopment())
            {
                loggerConfig
                    .MinimumLevel.Debug()
                    .WriteTo.RollingFile("logs\\DotCom-{Date}.txt")
                    .WriteTo.Console();
            }
            else
            {
                loggerConfig
                    .MinimumLevel.Debug()
                    .WriteTo.RollingFile("logs\\DotCom-{Date}.txt")
                    .WriteTo.Logentries(logentriesToken, restrictedToMinimumLevel: LogEventLevel.Warning);
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

        #endregion Methods
    }
}

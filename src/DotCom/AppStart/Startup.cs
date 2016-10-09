using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.AppStart;

namespace OwnApt.DotCom
{
    public class Startup
    {
        #region Public Constructors

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            OwnAptStartup.ConfigureOwnAptStartup(Configuration, env);
        }

        #endregion Public Constructors

        #region Public Properties

        public IConfigurationRoot Configuration { get; }

        #endregion Public Properties

        #region Public Methods

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime, IOptions<OpenIdConnectOptions> oidcOptions)
        {
            app.UseOwnAptConfiguration(loggerFactory, appLifetime, oidcOptions);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseOwnAptServices();
        }

        #endregion Public Methods
    }
}

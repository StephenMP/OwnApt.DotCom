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
        #region Constructors

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            OwnAptStartup.ConfigureOwnAptStartup(Configuration, env);
        }

        #endregion Constructors

        #region Properties

        public IConfigurationRoot Configuration { get; }

        #endregion Properties

        #region Methods

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime, IOptions<OpenIdConnectOptions> oidcOptions)
        {
            app.UseOwnAptConfiguration(loggerFactory, appLifetime, oidcOptions);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseOwnAptServices();
        }

        #endregion Methods
    }
}

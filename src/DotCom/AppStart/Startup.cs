using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DotCom.Services;
using OwnApt.DotCom.Services;
using RestSharp;
using RestSharp.Authenticators;
using DotCom.AppStart;
using Microsoft.Extensions.Options;

namespace DotCom
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            OwnAptStartup.ConfigureOwnAptBuild(Configuration);
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseOwnAptServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<OpenIdConnectOptions> oidcOptions)
        {
            app.UseOwnAptConfiguration(env, loggerFactory, oidcOptions);
        }
    }
}

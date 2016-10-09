using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace OwnApt.DotCom
{
    public class Program
    {
        #region Public Methods

        public static void Main(string[] args)
        {
            var hostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();

            if (args.Length == 1)
            {
                hostBuilder.UseUrls(args[0]);
            }

            hostBuilder.Build().Run();
        }

        #endregion Public Methods
    }
}

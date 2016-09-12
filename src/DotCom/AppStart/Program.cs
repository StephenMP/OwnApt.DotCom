using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace OwnApt.DotCom
{
    public class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("https://0.0.0.0:5000")
                .Build();

            host.Run();
        }

        #endregion Methods
    }
}

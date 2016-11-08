using Microsoft.Extensions.Logging;
using Serilog;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class LoggerFactoryMockBuilder
    {
        #region Public Methods

        public static LoggerFactoryMockBuilder New() => new LoggerFactoryMockBuilder();

        public ILoggerFactory Build()
        {
            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .MinimumLevel.Debug()
                            .WriteTo.Async(a => a.Console())
                            .CreateLogger();

            return new LoggerFactory().AddSerilog();
        }

        #endregion Public Methods
    }
}

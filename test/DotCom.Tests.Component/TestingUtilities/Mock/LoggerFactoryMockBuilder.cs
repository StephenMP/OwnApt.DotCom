using Microsoft.Extensions.Logging;
using Serilog;

namespace DotCom.Tests.Component.TestingUtilities.Mock
{
    public class LoggerFactoryMockBuilder : MockBuilder<ILoggerFactory>
    {
        #region Public Methods

        public static LoggerFactoryMockBuilder New() => new LoggerFactoryMockBuilder();

        private LoggerFactoryMockBuilder()
        {
            Log.Logger = new LoggerConfiguration()
                           .Enrich.FromLogContext()
                           .MinimumLevel.Debug()
                           .WriteTo.Async(a => a.Console())
                           .CreateLogger();
        }

        public override ILoggerFactory Build()
        {
            return new LoggerFactory().AddSerilog();
        }

        #endregion Public Methods
    }
}

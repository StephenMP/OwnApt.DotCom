using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class LoggerFactoryMockBuilder : MockBuilder<ILoggerFactory>
    {
        public static LoggerFactoryMockBuilder NewBuilder() => new LoggerFactoryMockBuilder();

        public LoggerFactoryMockBuilder AddSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Async(a => a.Console())
                .CreateLogger();

            this.Mock.Object.AddSerilog();

            return this;
        }
    }
}

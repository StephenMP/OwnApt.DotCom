using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Serilog.Core;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class LoggerFactoryMockBuilder
    {
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
    }
}

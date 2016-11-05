using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class OptionsMockBuilder<TSettings> : MockBuilder<IOptions<TSettings>> where TSettings : class, new()
    {
        public static OptionsMockBuilder<TSettings> New() => new OptionsMockBuilder<TSettings>();
        public OptionsMockBuilder<TSettings> Value(TSettings value)
        {
            this.Mock.Setup(m => m.Value).Returns(value);
            return this;
        }
    }
}

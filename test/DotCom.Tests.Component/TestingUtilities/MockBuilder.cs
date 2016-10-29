using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class MockBuilder<TMock> where TMock : class
    {
        public Mock<TMock> Mock { get; }

        public MockBuilder()
        {
            this.Mock = new Mock<TMock>();
        }

        public virtual TMock Build()
        {
            return this.Mock.Object;
        }
    }
}

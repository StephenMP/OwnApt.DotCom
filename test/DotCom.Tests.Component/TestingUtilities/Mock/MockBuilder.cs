using System;
using Moq;

namespace DotCom.Tests.Component.TestingUtilities.Mock
{
    public class MockBuilder<TMock> where TMock : class
    {
        #region Public Constructors

        protected MockBuilder()
        {
            this.Mock = new Mock<TMock>();
        }

        #endregion Public Constructors

        #region Public Properties

        public Mock<TMock> Mock { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual TMock Build()
        {
            return this.Mock.Object;
        }

        #endregion Public Methods
    }
}

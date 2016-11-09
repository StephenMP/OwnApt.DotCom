using Microsoft.Extensions.Options;

namespace DotCom.Tests.Component.TestingUtilities.Mock
{
    public class OptionsMockBuilder<TSettings> : MockBuilder<IOptions<TSettings>> where TSettings : class, new()
    {
        #region Public Methods

        public static OptionsMockBuilder<TSettings> New() => new OptionsMockBuilder<TSettings>();

        public OptionsMockBuilder<TSettings> Value(TSettings value)
        {
            this.Mock.Setup(m => m.Value).Returns(value);
            return this;
        }

        #endregion Public Methods
    }
}

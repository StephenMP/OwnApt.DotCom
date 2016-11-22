using System.Threading.Tasks;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    public class SignUpServiceFeatures
    {
        #region Private Fields

        private SignUpServiceSteps steps;

        #endregion Private Fields

        #region Public Constructors

        public SignUpServiceFeatures()
        {
            this.steps = new SignUpServiceSteps();
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public async Task CanCreateSignUpTokenAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveAMockedSignUpService();

            await this.steps.WhenICreateSignUpToken();

            this.steps.ThenICanVerifyICreateToken();
        }

        [Fact]
        public async Task CanParseSignUpTokenAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveAMockedSignUpService();
            await this.steps.GivenIHaveATokenToParse();

            await this.steps.WhenIParseSignUpTokenAsync();

            this.steps.ThenICanVerifyICanParseSignUpToken();
        }

        #endregion Public Methods
    }
}

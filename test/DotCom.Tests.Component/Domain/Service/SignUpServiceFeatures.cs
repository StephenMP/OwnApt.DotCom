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
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveAMockedSignUpService();

            await this.steps.When.ICreateSignUpToken();

            this.steps.Then.ICanVerifyICreateToken();
        }

        [Fact]
        public async Task CanParseSignUpTokenAsync()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveAMockedSignUpService();
            await this.steps.Given.IHaveATokenToParse();

            await this.steps.When.IParseSignUpTokenAsync();

            this.steps.Then.ICanVerifyICanParseSignUpToken();
        }

        #endregion Public Methods
    }
}

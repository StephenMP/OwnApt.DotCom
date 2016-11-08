using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    public class SignUpServiceFeatures
    {
        private SignUpServiceSteps steps;

        public SignUpServiceFeatures()
        {
            this.steps = new SignUpServiceSteps();
        }

        [Fact]
        public async Task CanCreateSignUpTokenAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIAHaveMockedSignUpService();

            await this.steps.WhenICreateSignUpToken();

            this.steps.ThenICanVerifyICreateToken();
        }

        [Fact]
        public async Task CanParseSignUpTokenAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIAHaveMockedSignUpService();
            await this.steps.GivenIHaveATokenToParse();

            await this.steps.WhenIParseSignUpTokenAsync();

            this.steps.ThenICanVerifyICanParseSignUpToken();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DotCom.Tests.Component.TestingUtilities;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Dto.Account;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    public class SignUpServiceSteps : Steps
    {
        private string propertyId;
        private ISignUpService signupService;
        private string token;
        private SignUpTokenDto signUpToken;

        internal void GivenIAHaveMockedSignUpService()
        {
            var mailGunRestClient = MailGunRestClientMockBuilder
                                        .New()
                                        .ExecuteAny(HttpStatusCode.OK)
                                        .Build();

            var signUpLoggerFactory = LoggerFactoryMockBuilder
                                        .New()
                                        .Build();

            this.signupService = new SignUpService(mailGunRestClient, signUpLoggerFactory);
        }

        internal async Task GivenIHaveATokenToParse()
        {
            this.token = await this.signupService.CreateTokenAsync(this.propertyId);
        }

        internal void ThenICanVerifyICanParseSignUpToken()
        {
            Assert.NotNull(this.signUpToken);
            Assert.Equal(this.propertyId, this.signUpToken.PropertyIds[0]);
        }

        internal async Task WhenIParseSignUpTokenAsync()
        {
            this.signUpToken = await this.signupService.ParseTokenAsync(this.token);
        }

        internal void ThenICanVerifyICreateToken()
        {
            Assert.NotNull(this.token);
            Assert.NotEmpty(this.token);
        }

        internal void GivenIHaveAPropertyId()
        {
            this.propertyId = TestRandom.String;
        }

        internal async Task WhenICreateSignUpToken()
        {
            this.token = await this.signupService.CreateTokenAsync(this.propertyId);
        }
    }
}

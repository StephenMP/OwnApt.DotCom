using System.Net;
using System.Threading.Tasks;
using DotCom.Tests.Component.TestingUtilities;
using DotCom.Tests.Component.TestingUtilities.Mock;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Dto.Account;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    public class SignUpServiceSteps : Steps
    {
        #region Internal Fields

        internal string propertyId;
        internal ISignUpService signupService;
        internal SignUpTokenDto signUpToken;
        internal string token;

        #endregion Internal Fields

        #region Public Methods

        public void GivenIHaveAMockedSignUpService()
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

        public void GivenIHaveAPropertyId()
        {
            this.propertyId = TestRandom.String;
        }

        public async Task GivenIHaveATokenToParse()
        {
            this.token = await this.signupService.CreateTokenAsync(this.propertyId);
        }

        public void ThenICanVerifyICanParseSignUpToken()
        {
            Assert.NotNull(this.signUpToken);
            Assert.Equal(this.propertyId, this.signUpToken.PropertyIds[0]);
        }

        public async Task WhenICreateSignUpToken()
        {
            this.token = await this.signupService.CreateTokenAsync(this.propertyId);
        }

        public async Task WhenIParseSignUpTokenAsync()
        {
            this.signUpToken = await this.signupService.ParseTokenAsync(this.token);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void ThenICanVerifyICreateToken()
        {
            Assert.NotNull(this.token);
            Assert.NotEmpty(this.token);
        }

        #endregion Internal Methods
    }
}

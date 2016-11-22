using System.Net;
using System.Threading.Tasks;
using DotCom.Tests.Component.TestingUtilities;
using DotCom.Tests.Component.TestingUtilities.Mock;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Service;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    internal class AccountDomainServiceSteps : Steps
    {
        #region Internal Fields

        internal IAccountDomainService accountDomainService;
        internal string ownerEmail;
        internal string ownerId;
        internal OwnerModel ownerModelResult;
        internal OwnerModel ownerToCreate;
        internal string propertyId;
        internal ISignUpService signupService;
        internal string token;
        internal bool validateSignUpTokenResult;

        #endregion Internal Fields

        #region Public Constructors

        public AccountDomainServiceSteps()
        {
        }

        #endregion Public Constructors

        #region Internal Methods

        internal void GivenIHaveAnAccountDomainService()
        {
            this.accountDomainService = new AccountDomainService(this.signupService, this.proxy, this.mapper, this.serviceUriOptions, this.loggerFactory);
        }

        internal void GivenIHaveAnOwnerToCreate()
        {
            this.ownerId = TestRandom.String;
            this.ownerEmail = TestRandom.String;

            this.ownerToCreate = new OwnerModel
            {
                Id = this.ownerId,
                Contact = new ContactModel
                {
                    Email = this.ownerEmail
                }
            };
        }

        internal void GivenIHaveAPropertyId()
        {
            this.propertyId = TestRandom.String;
        }

        internal void GivenIHaveASignUpTokenString()
        {
            this.token = this.signupService.CreateTokenAsync(this.propertyId).Result;
        }

        internal void GivenIHaveMockedSignUpService()
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

        internal void ThenICanVerifyICanRegisterSignUpTokenAsync()
        {
            this.asyncActionToExecute();
        }

        internal void ThenICanVerifyICanUpdateOwnerPropertyIdsAsync()
        {
            this.asyncActionToExecute();
        }

        internal void ThenICanVerifyICanValidateSignUpTokenAsync()
        {
            Assert.True(this.validateSignUpTokenResult);
        }

        internal void ThenICanVerifyICreateOwner()
        {
            Assert.NotNull(this.ownerModelResult);
            Assert.Equal(this.ownerToCreate, this.ownerModelResult);
        }

        internal async Task WhenICreateOwnerAsync()
        {
            this.ownerModelResult = await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail);
        }

        internal void WhenICreateOwnerAsyncAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail); };
        }

        internal async Task WhenICreateOwnerAsyncFromToken()
        {
            this.ownerModelResult = await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail, this.token);
        }

        internal void WhenICreateOwnerAsyncFromTokenAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail, this.token); };
        }

        internal void WhenIRegisterSignupTokenAsync()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.RegisterSignUpTokenAsync(this.token); };
        }

        internal void WhenIUpdateOwnerPropertyIdsAsync()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.UpdateOwnerPropertyIdsAsync(this.ownerId, this.token); };
        }

        internal void WhenIUpdateOwnerPropertyIdsAsyncAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.UpdateOwnerPropertyIdsAsync(this.ownerId, this.token); };
        }

        internal async Task WhenIValidateSignUpTokenAsync()
        {
            this.validateSignUpTokenResult = await this.accountDomainService.ValidateSignUpTokenAsync(this.token);
        }

        internal void WhenIValidateSignUpTokenAsyncAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.ValidateSignUpTokenAsync(this.token); };
        }

        #endregion Internal Methods
    }
}

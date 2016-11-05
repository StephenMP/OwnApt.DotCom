using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DotCom.Tests.Component.TestingUtilities;
using OwnApt.Api.Contract.Model;
using OwnApt.Common.Extension;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Dto.Account;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    public class AccountDomainServiceFeatures
    {
        #region Private Fields

        private readonly AccountDomainServiceSteps steps;

        #endregion Private Fields

        #region Public Constructors

        public AccountDomainServiceFeatures()
        {
            this.steps = new AccountDomainServiceSteps();
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public async Task CanCreateOwnerAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAnOwnerToCreate();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveAMockedProxy<OwnerModel, OwnerModel>(true, this.steps.ownerToCreate);
            this.steps.GivenIHaveAnAccountDomainService();

            await this.steps.WhenICreateOwnerAsync();

            this.steps.ThenICanVerifyICreateOwner();
        }

        [Fact]
        public async Task CannotCreateOwnerDueToApiIssue()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAnOwnerToCreate();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveAMockedProxy<OwnerModel, OwnerModel>(false);
            this.steps.GivenIHaveAnAccountDomainService();

            this.steps.WhenICreateOwnerAsyncAction();

            await this.steps.ThenICanVerifyIThrowAsync<Exception>();
        }

        [Fact]
        public async Task CanCreateOwnerAsyncFromToken()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAnOwnerToCreate();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpTokenString();
            this.steps.GivenIHaveAMockedProxy<OwnerModel, OwnerModel>(true, this.steps.ownerToCreate);
            this.steps.GivenIHaveAnAccountDomainService();

            await this.steps.WhenICreateOwnerAsyncFromToken();

            this.steps.ThenICanVerifyICreateOwner();
        }

        [Fact]
        public async Task CannotCreateOwnerAsyncFromTokenDueToApiIssue()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAnOwnerToCreate();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpTokenString();
            this.steps.GivenIHaveAMockedProxy<OwnerModel, OwnerModel>(false);
            this.steps.GivenIHaveAnAccountDomainService();

            this.steps.WhenICreateOwnerAsyncFromTokenAction();

            await this.steps.ThenICanVerifyIThrowAsync<Exception>();
        }

        [Fact]
        public void CanRegisterSignUpTokenAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpTokenString();
            this.steps.GivenIHaveAMockedProxy<RegisteredTokenModel, RegisteredTokenModel>(true);
            this.steps.GivenIHaveAnAccountDomainService();

            this.steps.WhenIRegisterSignupTokenAsync();

            this.steps.ThenICanVerifyICanRegisterSignUpTokenAsync();
        }

        [Fact]
        public void CanUpdateOwnerPropertyIdsAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpTokenString();
            this.steps.GivenIHaveAMockedProxy<Missing, OwnerModel>(true);
            this.steps.GivenIHaveAnAccountDomainService();

            this.steps.WhenIUpdateOwnerPropertyIdsAsync();

            this.steps.ThenICanVerifyICanUpdateOwnerPropertyIdsAsync();
        }

        [Fact]
        public async Task CannotUpdateOwnerPropertyIdsAsyncDueToApiIssue()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpTokenString();
            this.steps.GivenIHaveAMockedProxy<Missing, OwnerModel>(false);
            this.steps.GivenIHaveAnAccountDomainService();

            this.steps.WhenIUpdateOwnerPropertyIdsAsyncAction();

            await this.steps.ThenICanVerifyIThrowAsync<Exception>();
        }

        [Fact]
        public async Task CanValidateSignUpTokenAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpTokenString();
            this.steps.GivenIHaveAMockedProxy<RegisteredTokenModel, RegisteredTokenModel>(true);
            this.steps.GivenIHaveAnAccountDomainService();

            await this.steps.WhenIValidateSignUpTokenAsync();

            this.steps.ThenICanVerifyICanValidateSignUpTokenAsync();
        }

        [Fact]
        public async Task CannotValidateSignUpTokenAsyncDueToApiIssue()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpTokenString();
            this.steps.GivenIHaveAMockedProxy<RegisteredTokenModel, RegisteredTokenModel>(false);
            this.steps.GivenIHaveAnAccountDomainService();

            this.steps.WhenIValidateSignUpTokenAsyncAction();

            await this.steps.ThenICanVerifyIThrowAsync<Exception>();
        }

        #endregion Public Methods
    }

    internal class AccountDomainServiceSteps : Steps
    {
        #region Internal Fields

        internal OwnerModel ownerToCreate;

        #endregion Internal Fields

        #region Private Fields

        private IAccountDomainService accountDomainService;
        private string ownerEmail;
        private string ownerId;
        private OwnerModel ownerModelResult;
        private string propertyId;
        private ISignUpService signupService;
        private string token;
        private bool validateSignUpTokenResult;

        #endregion Private Fields

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

        internal void ThenICanVerifyICreateOwner()
        {
            Assert.NotNull(this.ownerModelResult);
            Assert.Equal(this.ownerToCreate, this.ownerModelResult);
        }

        internal void ThenICanVerifyICanRegisterSignUpTokenAsync()
        {
            this.ExecuteAction();
        }

        internal async Task WhenICreateOwnerAsync()
        {
            this.ownerModelResult = await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail);
        }

        internal async Task WhenICreateOwnerAsyncFromToken()
        {
            this.ownerModelResult = await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail, this.token);
        }

        internal void WhenIRegisterSignupTokenAsync()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.RegisterSignUpTokenAsync(this.token); };
        }

        private void ExecuteAction()
        {
            // If no exception throws, we are good to go!
            this.asyncActionToExecute();
        }

        internal void WhenIUpdateOwnerPropertyIdsAsync()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.UpdateOwnerPropertyIdsAsync(this.ownerId, this.token); };
        }

        internal void ThenICanVerifyICanUpdateOwnerPropertyIdsAsync()
        {
            this.ExecuteAction();
        }

        internal async Task WhenIValidateSignUpTokenAsync()
        {
            this.validateSignUpTokenResult = await this.accountDomainService.ValidateSignUpTokenAsync(this.token);
        }

        internal void ThenICanVerifyICanValidateSignUpTokenAsync()
        {
            Assert.True(this.validateSignUpTokenResult);
        }

        internal void WhenICreateOwnerAsyncAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail); };
        }

        internal void WhenICreateOwnerAsyncFromTokenAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail, this.token); };
        }

        internal void WhenIValidateSignUpTokenAsyncAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.ValidateSignUpTokenAsync(this.token); };
        }

        internal void WhenIUpdateOwnerPropertyIdsAsyncAction()
        {
            this.asyncActionToExecute = async () => { await this.accountDomainService.UpdateOwnerPropertyIdsAsync(this.ownerId, this.token); };
        }

        #endregion Internal Methods
    }
}

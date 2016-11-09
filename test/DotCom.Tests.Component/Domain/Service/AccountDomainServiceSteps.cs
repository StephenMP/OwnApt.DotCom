using System.Net;
using System.Threading.Tasks;
using DotCom.Tests.Component.TestingUtilities;
using DotCom.Tests.Component.TestingUtilities.Mock;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Service;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
    internal class AccountDomainServiceGiven : Given
    {
        #region Private Fields

        private AccountDomainServiceSteps steps;

        #endregion Private Fields

        #region Public Constructors

        public AccountDomainServiceGiven(AccountDomainServiceSteps steps) : base(steps)
        {
            this.steps = steps;
        }

        #endregion Public Constructors

        #region Internal Methods

        internal void IHaveAnAccountDomainService()
        {
            this.steps.accountDomainService = new AccountDomainService(this.steps.signupService, this.baseSteps.proxy, this.baseSteps.mapper, this.baseSteps.serviceUriOptions, this.baseSteps.loggerFactory);
        }

        internal void IHaveAnOwnerToCreate()
        {
            this.steps.ownerId = TestRandom.String;
            this.steps.ownerEmail = TestRandom.String;

            this.steps.ownerToCreate = new OwnerModel
            {
                Id = this.steps.ownerId,
                Contact = new ContactModel
                {
                    Email = this.steps.ownerEmail
                }
            };
        }

        internal void IHaveAPropertyId()
        {
            this.steps.propertyId = TestRandom.String;
        }

        internal void IHaveASignUpTokenString()
        {
            this.steps.token = this.steps.signupService.CreateTokenAsync(this.steps.propertyId).Result;
        }

        internal void IHaveMockedSignUpService()
        {
            var mailGunRestClient = MailGunRestClientMockBuilder
                                        .New()
                                        .ExecuteAny(HttpStatusCode.OK)
                                        .Build();

            var signUpLoggerFactory = LoggerFactoryMockBuilder
                                        .New()
                                        .Build();

            this.steps.signupService = new SignUpService(mailGunRestClient, signUpLoggerFactory);
        }

        #endregion Internal Methods
    }

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
            this.Given = new AccountDomainServiceGiven(this);
            this.When = new AccountDomainServiceWhen(this);
            this.Then = new AccountDomainServiceThen(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public AccountDomainServiceGiven Given { get; }
        public AccountDomainServiceThen Then { get; }
        public AccountDomainServiceWhen When { get; }

        #endregion Public Properties
    }

    internal class AccountDomainServiceThen : Then
    {
        #region Private Fields

        private AccountDomainServiceSteps steps;

        #endregion Private Fields

        #region Public Constructors

        public AccountDomainServiceThen(AccountDomainServiceSteps steps) : base(steps)
        {
            this.steps = steps;
        }

        #endregion Public Constructors

        #region Internal Methods

        internal void ICanVerifyICanRegisterSignUpTokenAsync()
        {
            this.baseSteps.asyncActionToExecute();
        }

        internal void ICanVerifyICanUpdateOwnerPropertyIdsAsync()
        {
            this.baseSteps.asyncActionToExecute();
        }

        internal void ICanVerifyICanValidateSignUpTokenAsync()
        {
            Assert.True(this.steps.validateSignUpTokenResult);
        }

        internal void ICanVerifyICreateOwner()
        {
            Assert.NotNull(this.steps.ownerModelResult);
            Assert.Equal(this.steps.ownerToCreate, this.steps.ownerModelResult);
        }

        #endregion Internal Methods
    }

    internal class AccountDomainServiceWhen : When
    {
        #region Private Fields

        private AccountDomainServiceSteps steps;

        #endregion Private Fields

        #region Public Constructors

        public AccountDomainServiceWhen(AccountDomainServiceSteps steps) : base(steps)
        {
            this.steps = steps;
        }

        #endregion Public Constructors

        #region Internal Methods

        internal async Task ICreateOwnerAsync()
        {
            this.steps.ownerModelResult = await this.steps.accountDomainService.CreateOwnerAsync(this.steps.ownerId, this.steps.ownerEmail);
        }

        internal void ICreateOwnerAsyncAction()
        {
            this.baseSteps.asyncActionToExecute = async () => { await this.steps.accountDomainService.CreateOwnerAsync(this.steps.ownerId, this.steps.ownerEmail); };
        }

        internal async Task ICreateOwnerAsyncFromToken()
        {
            this.steps.ownerModelResult = await this.steps.accountDomainService.CreateOwnerAsync(this.steps.ownerId, this.steps.ownerEmail, this.steps.token);
        }

        internal void ICreateOwnerAsyncFromTokenAction()
        {
            this.baseSteps.asyncActionToExecute = async () => { await this.steps.accountDomainService.CreateOwnerAsync(this.steps.ownerId, this.steps.ownerEmail, this.steps.token); };
        }

        internal void IRegisterSignupTokenAsync()
        {
            this.baseSteps.asyncActionToExecute = async () => { await this.steps.accountDomainService.RegisterSignUpTokenAsync(this.steps.token); };
        }

        internal void IUpdateOwnerPropertyIdsAsync()
        {
            this.baseSteps.asyncActionToExecute = async () => { await this.steps.accountDomainService.UpdateOwnerPropertyIdsAsync(this.steps.ownerId, this.steps.token); };
        }

        internal void IUpdateOwnerPropertyIdsAsyncAction()
        {
            this.baseSteps.asyncActionToExecute = async () => { await this.steps.accountDomainService.UpdateOwnerPropertyIdsAsync(this.steps.ownerId, this.steps.token); };
        }

        internal async Task IValidateSignUpTokenAsync()
        {
            this.steps.validateSignUpTokenResult = await this.steps.accountDomainService.ValidateSignUpTokenAsync(this.steps.token);
        }

        internal void IValidateSignUpTokenAsyncAction()
        {
            this.baseSteps.asyncActionToExecute = async () => { await this.steps.accountDomainService.ValidateSignUpTokenAsync(this.steps.token); };
        }

        #endregion Internal Methods
    }
}

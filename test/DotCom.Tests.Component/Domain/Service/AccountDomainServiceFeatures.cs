using System;
using System.Reflection;
using System.Threading.Tasks;
using OwnApt.Api.Contract.Model;
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
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAnOwnerToCreate();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveAMockedProxy<OwnerModel, OwnerModel>(true, this.steps.ownerToCreate);
            this.steps.Given.IHaveAnAccountDomainService();

            await this.steps.When.ICreateOwnerAsync();

            this.steps.Then.ICanVerifyICreateOwner();
        }

        [Fact]
        public async Task CanCreateOwnerAsyncFromToken()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAnOwnerToCreate();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveASignUpTokenString();
            this.steps.Given.IHaveAMockedProxy<OwnerModel, OwnerModel>(true, this.steps.ownerToCreate);
            this.steps.Given.IHaveAnAccountDomainService();

            await this.steps.When.ICreateOwnerAsyncFromToken();

            this.steps.Then.ICanVerifyICreateOwner();
        }

        [Fact]
        public async Task CannotCreateOwnerAsyncFromTokenDueToApiIssue()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAnOwnerToCreate();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveASignUpTokenString();
            this.steps.Given.IHaveAMockedProxy<OwnerModel, OwnerModel>(false);
            this.steps.Given.IHaveAnAccountDomainService();

            this.steps.When.ICreateOwnerAsyncFromTokenAction();

            await this.steps.Then.ICanVerifyIThrowAsync<Exception>();
        }

        [Fact]
        public async Task CannotCreateOwnerDueToApiIssue()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAnOwnerToCreate();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveAMockedProxy<OwnerModel, OwnerModel>(false);
            this.steps.Given.IHaveAnAccountDomainService();

            this.steps.When.ICreateOwnerAsyncAction();

            await this.steps.Then.ICanVerifyIThrowAsync<Exception>();
        }

        [Fact]
        public async Task CannotUpdateOwnerPropertyIdsAsyncDueToApiIssue()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveASignUpTokenString();
            this.steps.Given.IHaveAMockedProxy<Missing, OwnerModel>(false);
            this.steps.Given.IHaveAnAccountDomainService();

            this.steps.When.IUpdateOwnerPropertyIdsAsyncAction();

            await this.steps.Then.ICanVerifyIThrowAsync<Exception>();
        }

        [Fact]
        public async Task CannotValidateSignUpTokenAsyncDueToApiIssue()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveASignUpTokenString();
            this.steps.Given.IHaveAMockedProxy<RegisteredTokenModel, RegisteredTokenModel>(false);
            this.steps.Given.IHaveAnAccountDomainService();

            this.steps.When.IValidateSignUpTokenAsyncAction();

            await this.steps.Then.ICanVerifyIThrowAsync<Exception>();
        }

        [Fact]
        public void CanRegisterSignUpTokenAsync()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveASignUpTokenString();
            this.steps.Given.IHaveAMockedProxy<RegisteredTokenModel, RegisteredTokenModel>(true);
            this.steps.Given.IHaveAnAccountDomainService();

            this.steps.When.IRegisterSignupTokenAsync();

            this.steps.Then.ICanVerifyICanRegisterSignUpTokenAsync();
        }

        [Fact]
        public void CanUpdateOwnerPropertyIdsAsync()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveASignUpTokenString();
            this.steps.Given.IHaveAMockedProxy<Missing, OwnerModel>(true);
            this.steps.Given.IHaveAnAccountDomainService();

            this.steps.When.IUpdateOwnerPropertyIdsAsync();

            this.steps.Then.ICanVerifyICanUpdateOwnerPropertyIdsAsync();
        }

        [Fact]
        public async Task CanValidateSignUpTokenAsync()
        {
            this.steps.Given.IHaveMockedSystemAndThirdPartyObjects();
            this.steps.Given.IHaveMockedSignUpService();
            this.steps.Given.IHaveAPropertyId();
            this.steps.Given.IHaveASignUpTokenString();
            this.steps.Given.IHaveAMockedProxy<RegisteredTokenModel, RegisteredTokenModel>(true);
            this.steps.Given.IHaveAnAccountDomainService();

            await this.steps.When.IValidateSignUpTokenAsync();

            this.steps.Then.ICanVerifyICanValidateSignUpTokenAsync();
        }

        #endregion Public Methods
    }
}

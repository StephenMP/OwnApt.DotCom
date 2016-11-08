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

        #endregion Public Methods
    }
}

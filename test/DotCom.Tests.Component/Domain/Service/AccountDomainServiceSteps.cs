using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DotCom.Tests.Component.TestingUtilities;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Service;
using Xunit;

namespace DotCom.Tests.Component.Domain.Service
{
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

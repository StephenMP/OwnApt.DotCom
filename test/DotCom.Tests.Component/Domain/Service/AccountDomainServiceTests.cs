using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DotCom.Tests.Component.TestingUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.AppStart;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Settings;
using OwnApt.RestfulProxy.Client;
using OwnApt.RestfulProxy.Interface;
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
        public async Task CanCreateOwnerAsyncFromToken()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAnOwnerToCreate();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveASignUpToken();
            this.steps.GivenIHaveAMockedProxy();
            this.steps.GivenIHaveAnAccountDomainService();

            await this.steps.WhenICreateOwnerAsyncFromToken();

            this.steps.ThenICanVerifyICreateOwner();
        }

        [Fact]
        public async Task CanCreateOwnerAsync()
        {
            this.steps.GivenIHaveMockedSystemAndThirdPartyObjects();
            this.steps.GivenIHaveMockedSignUpService();
            this.steps.GivenIHaveAnOwnerToCreate();
            this.steps.GivenIHaveAPropertyId();
            this.steps.GivenIHaveAMockedProxy();
            this.steps.GivenIHaveAnAccountDomainService();

            await this.steps.WhenICreateOwnerAsync();

            this.steps.ThenICanVerifyICreateOwner();
        }

        #endregion Public Methods
    }

    internal class AccountDomainServiceSteps
    {
        #region Private Fields

        private IAccountDomainService accountDomainService;
        private ILoggerFactory loggerFactory;
        private IMapper mapper;
        private string ownerEmail;
        private string ownerId;
        private OwnerModel ownerModelResult;
        private OwnerModel ownerToCreate;
        private string propertyId;
        private IProxy proxy;
        private IOptions<ServiceUris> serviceUriOptions;
        private ServiceUris serviceUris;
        private ISignUpService signupService;
        private string token;

        #endregion Private Fields

        #region Internal Methods

        internal void GivenIHaveAMockedProxy()
        {
            var mockedProxyResponse = new ProxyResponse<OwnerModel>
            {
                IsSuccessfulStatusCode = true,
                ResponseDto = this.ownerToCreate
            };

            this.proxy = ProxyMockBuilder
                            .NewBuilder()
                            .InvokeAsyncAny<OwnerModel, OwnerModel>(mockedProxyResponse)
                            .Build();
        }

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

        internal void GivenIHaveASignUpToken()
        {
            this.token = this.signupService.CreateTokenAsync(this.propertyId).Result;
        }

        internal void GivenIHaveMockedSignUpService()
        {
            var mailGunRestClient = MailGunRestClientMockBuilder
                                        .NewBuilder()
                                        .ExecuteAny(HttpStatusCode.OK)
                                        .Build();

            var signUpLoggerFactory = LoggerFactoryMockBuilder
                                        .NewBuilder()
                                        .AddSerilog()
                                        .Build();

            this.signupService = new SignUpService(mailGunRestClient, signUpLoggerFactory);
        }

        internal void GivenIHaveMockedSystemAndThirdPartyObjects()
        {
            this.mapper = OwnAptStartup.BuildAutoMapper();

            this.serviceUris = new ServiceUris { ApiBaseUri = "http://this.is/a/test" };
            this.serviceUriOptions = OptionsMockBuilder<ServiceUris>
                                        .NewBuilder()
                                        .Value(this.serviceUris)
                                        .Build();

            this.loggerFactory = LoggerFactoryMockBuilder
                                    .NewBuilder()
                                    .AddSerilog()
                                    .Build();
        }

        internal void ThenICanVerifyICreateOwner()
        {
            Assert.NotNull(this.ownerModelResult);
            Assert.Equal(this.ownerToCreate, this.ownerModelResult);
        }

        internal async Task WhenICreateOwnerAsyncFromToken()
        {
            this.ownerModelResult = await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail, this.token);
        }

        internal async Task WhenICreateOwnerAsync()
        {
            this.ownerModelResult = await this.accountDomainService.CreateOwnerAsync(this.ownerId, this.ownerEmail);
        }

        #endregion Internal Methods
    }
}

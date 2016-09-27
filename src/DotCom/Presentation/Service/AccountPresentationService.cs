using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.ProxyRequests.Owner;
using OwnApt.RestfulProxy.Interface;
using RestSharp;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Text;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IAccountPresentationService
    {
        #region Public Methods

        Task<IProxyResponse<OwnerModel>> CreateOwner(string ownerId);
        Task<bool> ValidateSignUpTokenAsync(string token);
        Task RegisterSignUpTokenAsync(string token);
        Task<IProxyResponse<Missing>> UpdateOwnerPropertyIds(string ownerId, string token);
        Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds);

        #endregion Public Methods
    }

    public class AccountPresentationService : IAccountPresentationService
    {
        #region Private Fields

        private readonly ILogger<AccountPresentationService> logger;
        private readonly IProxy proxy;
        private readonly ServiceUriSettings serviceUrisSettings;
        private readonly ISignUpService signUpService;

        #endregion Private Fields

        #region Public Constructors

        public AccountPresentationService(
            IProxy proxy,
            IOptions<ServiceUriSettings> serviceUris,
            ISignUpService signUpService,
            ILoggerFactory loggerFatory
        )
        {
            this.proxy = proxy;
            this.signUpService = signUpService;
            this.serviceUrisSettings = serviceUris.Value;
            this.logger = loggerFatory.CreateLogger<AccountPresentationService>();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IProxyResponse<OwnerModel>> CreateOwner(string ownerId)
        {
            var ownerModel = new OwnerModel
            {
                Id = ownerId
            };

            var createOwnerRequest = new CreateOwnerProxyRequest(serviceUrisSettings.ApiBaseUri, ownerModel);
            var createOwnerResult = await this.proxy.InvokeAsync(createOwnerRequest);

            return createOwnerResult;
        }

        public async Task<bool> ValidateSignUpTokenAsync(string token)
        {
            var signUpToken = await this.signUpService.ParseTokenAsync(token);
            var subToken = $"{signUpToken.Nonce}-{signUpToken.UtcDateIssued.ToFileTimeUtc()}";

            var request = new ReadRegisteredTokenProxyRequest(this.serviceUrisSettings.ApiBaseUri, subToken);
            var response = await this.proxy.InvokeAsync(request);

            if (response.IsSuccessfulStatusCode)
            {
                var isValid = await this.signUpService.ValidateTokenAsync(token) && response.ResponseDto == null;
                return isValid;
            }

            throw ExceptionUtility.RaiseException(response, this.logger);
        }

        public async Task<IProxyResponse<Missing>> UpdateOwnerPropertyIds(string ownerId, string token)
        {
            var readOwnerRequest = new ReadOwnerProxyRequest(this.serviceUrisSettings.ApiBaseUri, ownerId);
            var readOwnerResponse = await this.proxy.InvokeAsync(readOwnerRequest);

            if (readOwnerResponse.IsSuccessfulStatusCode)
            {
                var ownerModel = readOwnerResponse.ResponseDto;
                var signUpToken = await this.signUpService.ParseTokenAsync(token);

                /////////////////////////////////////////////////////////////////////////
                // TODO REMOVE                                                         //
                /////////////////////////////////////////////////////////////////////////
                ownerModel.PropertyIds = new System.Collections.Generic.List<string>();
                /////////////////////////////////////////////////////////////////////////

                ownerModel.PropertyIds.AddRange(signUpToken.PropertyIds);

                var updateOwnerRequest = new UpdateOwnerProxyRequest(this.serviceUrisSettings.ApiBaseUri, ownerModel);
                var updateResponse = await this.proxy.InvokeAsync(updateOwnerRequest);
                return updateResponse;
            }

            throw ExceptionUtility.RaiseException(readOwnerResponse, this.logger);
        }

        public Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds)
        {
            return this.signUpService.SendSignUpEmailAsync(name, email, propertyIds);
        }

        public async Task RegisterSignUpTokenAsync(string token)
        {
            var signUpToken = await this.signUpService.ParseTokenAsync(token);
            var subToken = $"{signUpToken.Nonce}-{signUpToken.UtcDateIssued.ToFileTimeUtc()}";
            var request = new CreateRegisteredTokenProxyRequest(this.serviceUrisSettings.ApiBaseUri, subToken);
            var response = await this.proxy.InvokeAsync(request);

            if (!response.IsSuccessfulStatusCode)
            {
                throw ExceptionUtility.RaiseException(response, this.logger);
            }
        }

        #endregion Public Methods
    }
}

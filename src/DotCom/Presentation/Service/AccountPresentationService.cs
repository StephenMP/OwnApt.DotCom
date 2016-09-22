using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.ProxyRequests.Owner;
using OwnApt.RestfulProxy.Interface;
using System.Reflection;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IAccountPresentationService
    {
        #region Public Methods

        Task<IProxyResponse<OwnerModel>> CreateOwner(string ownerId);

        Task<IProxyResponse<Missing>> UpdateOwnerPropertyIds(string ownerId, string token);

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
            ISignUpService signUpService,
            IOptions<ServiceUriSettings> serviceUris,
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

        public async Task<IProxyResponse<Missing>> UpdateOwnerPropertyIds(string ownerId, string token)
        {
            var readOwnerRequest = new ReadOwnerProxyRequest(this.serviceUrisSettings.ApiBaseUri, ownerId);
            var readOwnerResponse = await this.proxy.InvokeAsync(readOwnerRequest);

            if (readOwnerResponse.IsSuccessfulStatusCode)
            {
                var ownerModel = readOwnerResponse.ResponseDto;
                var signUpToken = await this.signUpService.ParseTokenAsync(token);
                ownerModel.PropertyIds.AddRange(signUpToken.PropertyIds);

                var updateOwnerRequest = new UpdateOwnerProxyRequest(this.serviceUrisSettings.ApiBaseUri, ownerModel);
                var updateResponse = await this.proxy.InvokeAsync(updateOwnerRequest);
                return updateResponse;
            }

            throw ExceptionUtility.RaiseException(readOwnerResponse, this.logger);
        }

        #endregion Public Methods
    }
}

using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.ProxyRequests.Owner;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.ProxyRequests.Owner;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IAccountPresentationService
    {
        Task<IProxyResponse<OwnerModel>> CreateOwner(ClaimsPrincipal user);
        Task<IProxyResponse<Missing>> UpdateOwnerPropertyIds(ClaimsPrincipal user, string token);
    }
    public class AccountPresentationService : IAccountPresentationService
    {
        private readonly IClaimsService claimsService;
        private readonly ServiceUriSettings serviceUrisSettings;
        private readonly ISignUpService signUpService;
        private readonly IProxy proxy;
        private readonly ILogger<AccountPresentationService> logger;

        public AccountPresentationService(
            IProxy proxy,
            ISignUpService signUpService,
            IClaimsService claimsService,
            IOptions<ServiceUriSettings> serviceUris,
            ILoggerFactory loggerFatory
        )
        {
            this.proxy = proxy;
            this.signUpService = signUpService;
            this.claimsService = claimsService;
            this.serviceUrisSettings = serviceUris.Value;
            this.logger = loggerFatory.CreateLogger<AccountPresentationService>();
        }

        public async Task<IProxyResponse<OwnerModel>> CreateOwner(ClaimsPrincipal user)
        {
            var ownerModel = new OwnerModel
            {
                Id = await this.claimsService.GetUserIdAsync(user.Claims)
            };

            var request = new CreateOwnerProxyRequest(serviceUrisSettings.ApiBaseUri, ownerModel);
            var result = await this.proxy.InvokeAsync(request);

            return result;
        }

        public async Task<IProxyResponse<Missing>> UpdateOwnerPropertyIds(ClaimsPrincipal user, string token)
        {
            var ownerId = await this.claimsService.GetUserIdAsync(user.Claims);
            var readOwnerRequest = new ReadOwnerProxyRequest(this.serviceUrisSettings.ApiBaseUri, ownerId);
            var ownerResult = await this.proxy.InvokeAsync(readOwnerRequest);

            if (ownerResult.IsSuccessfulStatusCode)
            {
                var owner = ownerResult.ResponseDto;
                var signUpToken = await this.signUpService.ParseTokenAsync(token);
                owner.PropertyIds.AddRange(signUpToken.PropertyIds);

                var updateOwnerRequest = new UpdateOwnerProxyRequest(this.serviceUrisSettings.ApiBaseUri, owner);
                var updateResult = await this.proxy.InvokeAsync(updateOwnerRequest);
                return updateResult;
            }

            var message = string.IsNullOrWhiteSpace(ownerResult.ResponseMessage) ? $"An unknown issue ocurred when attempting to read owner with id {ownerId}" : ownerResult.ResponseMessage;
            throw ExceptionUtility.RaiseException(message, this.logger, LogLevel.Error);
        }
    }
}

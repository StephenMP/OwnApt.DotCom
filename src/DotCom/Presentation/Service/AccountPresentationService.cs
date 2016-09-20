using DotCom.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.ProxyRequests;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotCom.Presentation.Service
{
    public interface IAccountPresentationService
    {
        Task<IProxyResponse<Missing>> MapUserToPropertiesAsync(ClaimsPrincipal user, string token);
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

        public async Task<IProxyResponse<Missing>> MapUserToPropertiesAsync(ClaimsPrincipal user, string token)
        {
            var signUpToken = await this.signUpService.ParseTokenAsync(token);

            if (signUpToken != null)
            {
                var userId = await this.claimsService.GetUserIdAsync(user.Claims);
                var result = await this.proxy.InvokeAsync(new MapOwnerToPropertiesProxyRequest(serviceUrisSettings.ApiBaseUri, userId, signUpToken));
                return result;
            }

            var message = $"The sign up token has been altered or has expired! Token recieved: {token}";
            throw ExceptionUtility.RaiseException(message, this.logger, LogLevel.Error);
        }
    }
}

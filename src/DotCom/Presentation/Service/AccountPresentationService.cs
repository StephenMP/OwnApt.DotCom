using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Extensions;
using OwnApt.DotCom.Model.Account;
using RestSharp;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IAccountPresentationService
    {
        #region Public Methods

        LockContextModel BuildLoginModel(HttpContext context, string returnUrl);

        LockContextModel BuildSignUpModel(HttpContext context, string returnUrl);

        Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail);

        Task RegisterSignUpTokenAsync(string token);

        Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds);

        Task UpdateOwnerPropertyIds(string ownerId, string token);

        Task<bool> ValidateSignUpTokenAsync(string token);

        #endregion Public Methods
    }

    public class AccountPresentationService : IAccountPresentationService
    {
        #region Private Fields

        private readonly IAccountDomainService accountDomainService;

        private readonly ILogger<AccountPresentationService> logger;

        private readonly OpenIdConnectOptions openIdConnectOptions;

        #endregion Private Fields

        #region Public Constructors

        public AccountPresentationService(
            IAccountDomainService accountDomainService,
            IOptions<OpenIdConnectOptions> openIdConnectOptions,
            ILoggerFactory loggerFatory
        )
        {
            this.accountDomainService = accountDomainService;
            this.openIdConnectOptions = openIdConnectOptions.Value;
            this.logger = loggerFatory.CreateLogger<AccountPresentationService>();
        }

        #endregion Public Constructors

        #region Public Methods

        public LockContextModel BuildLoginModel(HttpContext context, string returnUrl)
        {
            var lockContext = context.GenerateLockContext(openIdConnectOptions, returnUrl);
            return lockContext;
        }

        public LockContextModel BuildSignUpModel(HttpContext context, string returnUrl)
        {
            var lockContext = context.GenerateLockContext(openIdConnectOptions, returnUrl);
            return lockContext;
        }

        public Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail)
        {
            return this.accountDomainService.CreateOwner(ownerId, ownerEmail);
        }

        public Task RegisterSignUpTokenAsync(string token)
        {
            return this.accountDomainService.RegisterSignUpTokenAsync(token);
        }

        public Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds)
        {
            return this.accountDomainService.SendSignUpEmailAsync(name, email, propertyIds);
        }

        public Task UpdateOwnerPropertyIds(string ownerId, string token)
        {
            return this.accountDomainService.UpdateOwnerPropertyIds(ownerId, token);
        }

        public Task<bool> ValidateSignUpTokenAsync(string token)
        {
            return this.accountDomainService.ValidateSignUpTokenAsync(token);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<LockContextModel> GenerateLockContextAsync(HttpContext context, string returnUrl)
        {
            return await Task.FromResult(context.GenerateLockContext(this.openIdConnectOptions, returnUrl));
        }

        #endregion Private Methods
    }
}

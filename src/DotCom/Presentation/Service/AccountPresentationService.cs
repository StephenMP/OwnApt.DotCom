using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Extensions;
using OwnApt.DotCom.Model.Account;
using RestSharp;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IAccountPresentationService
    {
        #region Public Methods

        LockContextModel BuildLoginModel(HttpContext context, string returnUrl);

        LockContextModel BuildSignUpModel(HttpContext context, string returnUrl);

        Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail);

        Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail, string token);

        Task RegisterSignUpTokenAsync(string token);

        Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds);

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

        public async Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail)
        {
            return await this.accountDomainService.CreateOwner(ownerId, ownerEmail);
        }

        public async Task<OwnerModel> CreateOwner(string ownerId, string ownerEmail, string token)
        {
            return await this.accountDomainService.CreateOwner(ownerId, ownerEmail, token);
        }

        public async Task RegisterSignUpTokenAsync(string token)
        {
            await this.accountDomainService.RegisterSignUpTokenAsync(token);
        }

        public async Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds)
        {
            return await this.accountDomainService.SendSignUpEmailAsync(name, email, propertyIds);
        }

        public async Task<bool> ValidateSignUpTokenAsync(string token)
        {
            return await this.accountDomainService.ValidateSignUpTokenAsync(token);
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

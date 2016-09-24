using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Extensions;
using OwnApt.DotCom.Presentation.Service;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Controllers
{
    public class AccountController : Controller
    {
        #region Private Fields

        private readonly IAccountPresentationService accountPresentationService;

        private readonly IClaimsService claimsService;
        private readonly ILogger logger;
        private readonly OpenIdConnectOptions openIdConnectOptions;
        private readonly ISignUpService signUpService;

        #endregion Private Fields

        #region Public Constructors

        public AccountController(
            IAccountPresentationService accountPresentationService,
            ISignUpService signUpService,
            IClaimsService claimsService,
            IOptions<OpenIdConnectOptions> openIdOptions,
            ILoggerFactory loggerFatory
        )
        {
            this.accountPresentationService = accountPresentationService;
            this.openIdConnectOptions = openIdOptions.Value;
            this.signUpService = signUpService;
            this.claimsService = claimsService;
            this.logger = loggerFatory.CreateLogger<AccountController>();
        }

        #endregion Public Constructors

        /* REMOVE : HERE FOR TESTING PURPOSES ONLY */

        #region Public Methods

        [Authorize]
        public IActionResult Claims()
        {
            var user = User;
            return View();
        }

        /* REMOVE : HERE FOR TESTING PURPOSES ONLY */

        [HttpGet]
        public bool Email()
        {
            const string propId = "6395bfba2bd543e9bf2dd2b7618baf7a";

            this.signUpService.SendSignUpEmailAsync("John Doe", "1.stephen.porter@gmail.com", new string[] { propId });

            return true;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            var lockContext = HttpContext.GenerateLockContext(this.openIdConnectOptions, returnUrl);
            return View(lockContext);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Auth0");
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MapUserToProperties(string token)
        {
            var ownerId = await this.claimsService.GetUserIdAsync(User.Claims);
            var createOwnerResponse = await this.accountPresentationService.CreateOwner(ownerId);
            if (createOwnerResponse.IsSuccessfulStatusCode)
            {
                var updateOwnerPropertyResponse = await this.accountPresentationService.UpdateOwnerPropertyIds(ownerId, token);
                if (updateOwnerPropertyResponse.IsSuccessfulStatusCode)
                {
                    return RedirectToAction(nameof(Claims));
                }

                throw ExceptionUtility.RaiseException(updateOwnerPropertyResponse, this.logger);
            }

            throw ExceptionUtility.RaiseException(createOwnerResponse, this.logger);
        }

        // TODO : Update to signup using token to support ad hoc signups
        public async Task<IActionResult> SignUp(string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }

            this.logger.LogInformation($"Received sign up token: {token}");

            var tokenIsValid = await this.signUpService.ValidateTokenAsync(token);
            if (tokenIsValid)
            {
                this.logger.LogInformation($"Token sign up token is valid: {token}");
                var returnUrl = $"/Account/MapUserToProperties?token={token}";
                var lockContext = HttpContext.GenerateLockContext(this.openIdConnectOptions, returnUrl);
                return View(lockContext);
            }

            var message = $"The sign up token has been altered or has expired! Token recieved: {token}";
            throw ExceptionUtility.RaiseException(message, this.logger, LogLevel.Error);
        }

        #endregion Public Methods
    }
}

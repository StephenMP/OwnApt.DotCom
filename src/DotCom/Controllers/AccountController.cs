using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.Extensions;
using OwnApt.DotCom.ProxyRequests;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Controllers
{
    public class AccountController : Controller
    {
        #region Fields

        private readonly IClaimsService claimsService;
        private readonly ILogger logger;
        private readonly IOptions<OpenIdConnectOptions> options;
        private readonly IProxy proxy;
        private readonly IOptions<ServiceUriSettings> serviceUris;
        private readonly ISignUpService signUpService;

        #endregion Fields

        #region Constructors

        public AccountController(
            IProxy proxy,
            ISignUpService signUpService,
            IClaimsService claimsService,
            IOptions<OpenIdConnectOptions> options,
            IOptions<ServiceUriSettings> serviceUris,
            ILoggerFactory loggerFatory
        )
        {
            this.options = options;
            this.signUpService = signUpService;
            this.serviceUris = serviceUris;
            this.proxy = proxy;
            this.claimsService = claimsService;
            this.logger = loggerFatory.CreateLogger<AccountController>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the Auth0 ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            var user = User;
            return View();
        }

        [HttpGet]
        /* REMOVE : HERE FOR TESTING PURPOSES ONLY */
        public bool Email()
        {
            var propId = "f254534c48fb49168188c70c1108d75b";

            this.signUpService.SendSignUpEmailAsync("John Doe", "1.stephen.porter@gmail.com", new string[] { propId });

            return true;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            var lockContext = HttpContext.GenerateLockContext(this.options.Value, returnUrl);
            return View(lockContext);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userEmail = await this.claimsService.GetUserEmailAsync(User.Claims);
            this.logger.LogInformation($"User {userEmail} logging out");

            await HttpContext.Authentication.SignOutAsync("Auth0");
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MapUserToProperties(string token)
        {
            var signUpToken = await this.signUpService.ParseTokenAsync(token);
            var userId = await this.claimsService.GetUserIdAsync(User.Claims);
            var result = await this.proxy.InvokeAsync(new MapOwnerToPropertiesProxyRequest(serviceUris.Value.ApiBaseUri, userId, signUpToken));

            if (result.IsSuccessfulStatusCode)
            {
                return RedirectToAction(nameof(Claims));
            }

            var message = result.ResponseMessage ?? result.StatusCode.ToString();

            this.logger.LogCritical(message);
            throw new Exception(message);
        }

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
                this.logger.LogInformation($"Token sign up token {token} is valid");
                var returnUrl = $"/Account/MapUserToProperties?token={token}";
                var lockContext = HttpContext.GenerateLockContext(this.options.Value, returnUrl);
                return View(lockContext);
            }

            var message = $"The sign up token {token} has been altered or has expired!";
            this.logger.LogCritical(message);
            throw new Exception(message);
        }

        #endregion Methods
    }
}

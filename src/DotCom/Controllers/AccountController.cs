using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Presentation.Service;

namespace OwnApt.DotCom.Controllers
{
    public class AccountController : Controller
    {
        #region Private Fields

        private readonly IAccountPresentationService accountPresentationService;
        private readonly IClaimsService claimsService;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public AccountController(
            IAccountPresentationService accountPresentationService,
            IClaimsService claimsService,
            ILoggerFactory loggerFatory
        )
        {
            this.accountPresentationService = accountPresentationService;
            this.claimsService = claimsService;
            this.logger = loggerFatory.CreateLogger<AccountController>();
        }

        #endregion Public Constructors

        /* IT SUPPORT */

        #region Public Methods

        [HttpGet]
        [Authorize]
        public string Email()
        {
            /* Jason Email */
            var propId = "f45cf61f92c448ebbeb4f63ff8d7e0f3";
            this.accountPresentationService.SendSignUpEmailAsync("Jason", "realjforge@gmail.com", new string[] { propId });

            /* Shuwun Email */
            propId = "ed43c2981469482c926d3c4bdead53a8";
            this.accountPresentationService.SendSignUpEmailAsync("Shuwun", "shuwunma@gmail.com", new string[] { propId });

            /* Leon Email */
            propId = "9a3d5ed1adc84ff0825be5ae9ecebb01";
            this.accountPresentationService.SendSignUpEmailAsync("Leon", "leonrubalcava@gmail.com", new string[] { propId });

            /* Leon Email */
            propId = "9a3d5ed1adc84ff0825be5ae9ecebb01";
            this.accountPresentationService.SendSignUpEmailAsync("Alexis", "12.alexis.porter@gmail.com", new string[] { propId });

            /* Stephen Email */
            var propIds = new[] { "9a3d5ed1adc84ff0825be5ae9ecebb01", "f45cf61f92c448ebbeb4f63ff8d7e0f3", "ed43c2981469482c926d3c4bdead53a8" };
            this.accountPresentationService.SendSignUpEmailAsync("Stephen", "1.stephen.porter@gmail.com", propIds);

            return "Yo! That email was sent dawg!";
        }

        public IActionResult Login(string returnUrl = "/Owner/Index")
        {
            var loginModel = this.accountPresentationService.BuildLoginModel(HttpContext, returnUrl);
            return View(loginModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Auth0");
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> MapUserToProperties(string token)
        {
            var ownerId = await this.claimsService.GetUserIdAsync(User.Claims);
            var ownerEmail = await this.claimsService.GetUserEmailAsync(User.Claims);

            await this.accountPresentationService.CreateOwner(ownerId, ownerEmail, token);
            await this.accountPresentationService.RegisterSignUpTokenAsync(token);

            return RedirectToAction("Index", "Owner");
        }

        public async Task<IActionResult> SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Owner");
            }

            var returnUrl = $"/Owner/Index";
            var signUpModel = await Task.Run(() => this.accountPresentationService.BuildSignUpModel(HttpContext, returnUrl));

            return View(signUpModel);
        }

        // TODO : Update to signup using token to support ad hoc signups
        public async Task<IActionResult> SignUpFromToken(string token)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Owner");
            }

            var tokenIsValid = await this.accountPresentationService.ValidateSignUpTokenAsync(token);
            if (tokenIsValid)
            {
                var returnUrl = $"/Account/MapUserToProperties?token={token}";
                var signUpModel = this.accountPresentationService.BuildSignUpModel(HttpContext, returnUrl);

                return View("SignUp", signUpModel);
            }

            var message = $"The sign up token has been altered, reused or has expired! Token recieved: {token}";
            throw ExceptionUtility.RaiseException(message, this.logger, LogLevel.Error);
        }

        #endregion Public Methods
    }
}

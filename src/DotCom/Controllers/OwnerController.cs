using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Presentation.Service;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Controllers
{
    [Authorize]
    public class OwnerController : Controller
    {
        #region Private Fields

        private readonly ILogger<OwnerController> logger;
        private readonly IOwnerPresentationService ownerPresentationService;
        private readonly IClaimsService claimsService;

        #endregion Private Fields

        #region Public Constructors

        public OwnerController(
            IOwnerPresentationService ownerPresentationService,
            IClaimsService claimsService,
            ILoggerFactory loggerFactory
        )
        {
            this.ownerPresentationService = ownerPresentationService;
            this.claimsService = claimsService;
            this.logger = loggerFactory.CreateLogger<OwnerController>();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IActionResult> Index()
        {
            var ownerId = await this.claimsService.GetUserIdAsync(User.Claims);
            var model = await this.ownerPresentationService.BuildIndexModelAsync(ownerId);
            return View(model);
        }

        #endregion Public Methods
    }
}

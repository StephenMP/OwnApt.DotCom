using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Presentation.Service;

namespace OwnApt.DotCom.Controllers
{
    [Authorize]
    public class OwnerController : Controller
    {
        #region Private Fields

        private readonly IClaimsService claimsService;
        private readonly ILogger<OwnerController> logger;
        private readonly IOwnerPresentationService ownerPresentationService;

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
            var ownerId = this.claimsService.GetUserId(User);
            var model = await this.ownerPresentationService.BuildIndexModelAsync(ownerId);
            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var ownerId = this.claimsService.GetUserId(User);
            var model = await this.ownerPresentationService.BuildProfileModelAsync(ownerId);
            return View(model);
        }

        #endregion Public Methods
    }
}

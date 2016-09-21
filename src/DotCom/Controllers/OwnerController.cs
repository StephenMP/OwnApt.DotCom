using OwnApt.DotCom.Presentation.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Controllers
{
    [Authorize]
    public class OwnerController : Controller
    {
        private readonly ILogger<OwnerController> logger;
        private readonly IOwnerPresentationService ownerPresentationService;

        public OwnerController(
            IOwnerPresentationService ownerPresentationService,
            ILoggerFactory loggerFactory
        )
        {
            this.ownerPresentationService = ownerPresentationService;
            this.logger = loggerFactory.CreateLogger<OwnerController>();
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.ownerPresentationService.BuildIndexModelAsync(User);
            return View(model);
        }
    }
}

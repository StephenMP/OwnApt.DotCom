using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Model.Home;
using OwnApt.DotCom.Settings;
using OwnApt.DotCom.ViewModels.Dto;
using System.Net;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Controllers
{
    public class HomeController : Controller
    {
        #region Private Fields

        private readonly IContactFormService emailSender;
        private readonly FeatureToggles featureToggles;

        #endregion Private Fields

        #region Public Constructors

        public HomeController(IContactFormService emailSender, IOptions<FeatureToggles> featureToggles)
        {
            this.emailSender = emailSender;
            this.featureToggles = featureToggles.Value;
        }

        #endregion Public Constructors

        #region Public Methods

        public IActionResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            return View("~/Views/Shared/Error.cshtml", error);
        }

        public IActionResult Index()
        {
            var model = new HomeIndexViewModel { FeatureToggles = this.featureToggles };
            return View(model);
        }

        public async Task<bool> SubmitFormAsync(ContactFormViewDto contactFormViewModel)
        {
            var meh = await this.emailSender.SendEmailAsync(contactFormViewModel.FullName, contactFormViewModel.ToString());
            return meh.StatusCode == HttpStatusCode.OK;
        }

        #endregion Public Methods
    }
}

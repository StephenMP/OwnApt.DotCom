using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Model.Home;
using OwnApt.DotCom.ViewModels.Dto;

namespace OwnApt.DotCom.Controllers
{
    public class HomeController : Controller
    {
        #region Private Fields

        private readonly IContactFormService emailSender;

        #endregion Private Fields

        #region Public Constructors

        public HomeController(IContactFormService emailSender)
        {
            this.emailSender = emailSender;
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
            var model = new HomeIndexViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> SubmitForm(ContactFormViewDto contactFormViewModel)
        {
            var meh = await this.emailSender.SendEmailAsync(contactFormViewModel.FullName, contactFormViewModel.ToString());
            return meh.StatusCode == HttpStatusCode.OK;
        }

        #endregion Public Methods
    }
}

using Microsoft.AspNetCore.Mvc;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.ViewModels.Dto;
using System.Net;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly IContactFormService emailSender;

        #endregion Fields

        #region Constructors

        public HomeController(IContactFormService emailSender)
        {
            this.emailSender = emailSender;
        }

        #endregion Constructors

        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        public async Task<bool> SubmitFormAsync(ContactFormViewDto contactFormViewModel)
        {
            var meh = await this.emailSender.SendEmailAsync(contactFormViewModel.FullName, contactFormViewModel.ToString());
            return meh.StatusCode == HttpStatusCode.OK;
        }

        #endregion Methods
    }
}

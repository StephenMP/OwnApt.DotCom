using DotCom.Services;
using Microsoft.AspNet.Mvc;
using OwnApt.DotCom.ViewModels.Home;
using System.Net;
using System.Threading.Tasks;

namespace DotCom.Controllers
{
    public class HomeController : Controller
    {
        private IEmailService emailSender;

        public HomeController(IEmailService emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<bool> SubmitForm(ContactFormViewModel contactFormViewModel)
        {
            var meh = await this.emailSender.SendEmailAsync(contactFormViewModel.FullName, contactFormViewModel.ToString());
            return meh.StatusCode == HttpStatusCode.OK;
        }
    }
}

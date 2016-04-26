using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Mail;
using System.Net;
using DotCom.Services;
using OwnApt.DotCom.ViewModels.Home;

namespace DotCom.Controllers
{
    public class HomeController : Controller
    {
        private IEmailSender emailSender;

        public HomeController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<bool> SubmitForm(ContactFormViewModel contactFormViewModel)
        {
            return await this.emailSender.SendEmailAsync(contactFormViewModel.FullName, contactFormViewModel.ToString());
        }
    }
}

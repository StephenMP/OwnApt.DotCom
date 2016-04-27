using DotCom.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Services
{
    public class ContactFormEmailSender : IEmailSender
    {
        public async Task<bool> SendEmailAsync(string name, string message)
        {
            var result = true;

            try {
                using (var client = new SmtpClient())
                {
                    using (var mailMessage = new MailMessage("ownapt.contact.form@gmail.com", "admin@ownapt.com", $"New Contact: {name}", message))
                    {

                        client.Host = "smtp.googlemail.com";
                        client.Port = 587;
                        client.UseDefaultCredentials = false;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.EnableSsl = true;
                        client.Credentials = new NetworkCredential("ownapt.contact.form@gmail.com", "trldbpjlzlhbzkof");
                        await client.SendMailAsync(mailMessage);
                    }
                }
            }

            catch
            {
                result = false;
            }

            return await Task.FromResult(result);
        }
    }
}

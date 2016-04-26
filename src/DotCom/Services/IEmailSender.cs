using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotCom.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string name, string message);
    }
}

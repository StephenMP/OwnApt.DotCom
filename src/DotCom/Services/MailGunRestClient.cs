using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotCom.Services
{
    public class MailGunRestClient : RestClient, IMailGunRestClient
    {
        public MailGunRestClient() : base() { }
        public MailGunRestClient(string baseUri) : base(baseUri) { }
        public MailGunRestClient(Uri baseUri) : base(baseUri) { }
    }
}

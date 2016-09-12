using OwnApt.DotCom.Domain.Interface;
using RestSharp;
using System;

namespace OwnApt.DotCom.Domain.Service
{
    public class MailGunRestClient : RestClient, IMailGunRestClient
    {
        #region Constructors

        public MailGunRestClient()
        {
        }

        public MailGunRestClient(string baseUri) : base(baseUri)
        {
        }

        public MailGunRestClient(Uri baseUri) : base(baseUri)
        {
        }

        #endregion Constructors
    }
}

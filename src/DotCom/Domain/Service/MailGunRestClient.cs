using System;

using RestSharp;

namespace OwnApt.DotCom.Domain.Service
{
    public interface IMailGunRestClient : IRestClient
    {
    }

    public class MailGunRestClient : RestClient, IMailGunRestClient
    {
        #region Public Constructors

        public MailGunRestClient()
        {
        }

        public MailGunRestClient(string baseUri) : base(baseUri)
        {
        }

        public MailGunRestClient(Uri baseUri) : base(baseUri)
        {
        }

        #endregion Public Constructors
    }
}

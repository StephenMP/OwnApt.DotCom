using OwnApt.DotCom.Domain.Interface;
using RestSharp;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Service
{
    public class ContactFormService : IContactFormService
    {
        #region Fields

        private readonly IMailGunRestClient restClient;

        #endregion Fields

        #region Constructors

        public ContactFormService(IMailGunRestClient restClient)
        {
            this.restClient = restClient;
        }

        #endregion Constructors

        #region Methods

        public async Task<IRestResponse> SendEmailAsync(string name, string message)
        {
            var request = new RestRequest("{domain}/messages", Method.POST);
            //request.AddParameter("domain", "sandbox8a317734176742dca88b5a4fa1663bda.mailgun.org", ParameterType.UrlSegment);
            request.AddParameter("domain", "mailgun.ownapt.com", ParameterType.UrlSegment);
            request.AddParameter("from", "Contact Form <mailgun@ownapt.com>");
            request.AddParameter("to", "admin@ownapt.com");
            request.AddParameter("subject", $"New Contact: {name}");
            request.AddParameter("text", message);

            return await Task.FromResult(restClient.Execute(request));
        }

        #endregion Methods
    }
}

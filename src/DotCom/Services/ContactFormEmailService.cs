using DotCom.Services;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Services
{
    public class ContactFormEmailService : IEmailService
    {
        private RestClient client;

        public ContactFormEmailService()
        {
            this.client = new RestClient("https://api.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator("api", "key-9d43cbe044c65948c59629ea2f280647")
            };
        }

        public async Task<IRestResponse> SendEmailAsync(string name, string message)
        {
            RestRequest request = new RestRequest("{domain}/messages", Method.POST);
            //request.AddParameter("domain", "sandbox8a317734176742dca88b5a4fa1663bda.mailgun.org", ParameterType.UrlSegment);
            request.AddParameter("domain", "mailgun.ownapt.com", ParameterType.UrlSegment);
            request.AddParameter("from", "Contact Form <mailgun@ownapt.com>");
            request.AddParameter("to", "admin@ownapt.com");
            request.AddParameter("subject", $"New Contact: {name}");
            request.AddParameter("text", message);

            return await client.ExecuteTaskAsync(request);
        }
        /* curl -s --user 'api:key-9d43cbe044c65948c59629ea2f280647' \
>     https://api.mailgun.net/v3/sandbox8a317734176742dca88b5a4fa1663bda.mailgun.org/messages \
>     -F from='Mailgun Sandbox <postmaster@sandbox8a317734176742dca88b5a4fa1663bda.mailgun.org>' \
>     -F to='OwnApt <admin@ownapt.com>' \
>     -F subject='Hello OwnApt' \
>     -F text='Congratulations OwnApt, you just sent an email with Mailgun!  You are truly awesome!'
{
*/
    }
}

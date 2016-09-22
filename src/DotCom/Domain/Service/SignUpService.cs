using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Interface;
using OwnApt.DotCom.Dto.Account;
using RestSharp;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Service
{
    public class SignUpService : ISignUpService
    {
        #region Private Fields

        private const string emailBody = "<html><head><style type=\"text/css\">.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td, img{{line-height:100%}}#outlook a{{padding:0}}.ExternalClass,.ReadMsgBody{{width:100%}}a,blockquote,body,li,p,table,td{{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}}table,td{{mso-table-lspace:0;mso-table-rspace:0}}img{{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}}table{{border-collapse:collapse!important}}#bodyCell,#bodyTable,body{{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}}#bodyCell{{padding:20px}}#bodyTable{{width:600px}}@font-face{{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}}@font-face{{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}}@media only screen and (max-width:480px){{#bodyTable,body{{width:100%!important}}a,blockquote,body,li,p,table,td{{-webkit-text-size-adjust:none!important}}body{{min-width:100%!important}}#bodyTable{{max-width:600px!important}}#signIn{{max-width:280px!important}}}}</style></head><body><center><table style=\"width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\" id=\"bodyTable\"><tr><td align=\"center\" valign=\"top\" id=\"bodyCell\" style=\"-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;\"><div class=\"main\"><p style=\"text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;\"><img src=\"http://ownapt.com/images/Logo/Logo_250x241.png\" width=\"50\" alt=\"OwnApt\" style=\"-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;\"></p><h1>Welcome to OwnApt!</h1><p>Please setup your account by clicking the following link within the next 72 hours:</p><p><a href=\"http://localhost:5000/Account/SignUp?token={0}\"> Create my account</a></p><p>If you have any questions, please don't hesitate to contact us by replying to this email or using any of the contact methods below.</p><br>Best Regards,<br><strong>The OwnApt Team</strong><div>Call or Text: 208.991.0492</div><div>Email: support @ownapt.com</div><br><br><hr style=\"border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;\"><p style= \"text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;\">If you did not make this request, please contact us as soon as possible.</p></div></td></tr></table></center></body></html>";
        private readonly ILogger logger;
        private readonly IMailGunRestClient restClient;

        #endregion Private Fields

        #region Public Constructors

        public SignUpService(IMailGunRestClient restClient, ILoggerFactory loggerFactory)
        {
            this.restClient = restClient;
            this.logger = loggerFactory.CreateLogger<SignUpService>();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<string> CreateTokenAsync(params string[] propertyIds)
        {
            var utcDateTime = DateTime.UtcNow;
            var nonce = Guid.NewGuid().ToString("N");

            var signUpToken = new SignUpTokenDto
            {
                Nonce = nonce,
                PropertyIds = propertyIds,
                UtcDateIssued = utcDateTime
            };

            var signUpTokenJson = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(signUpToken));
            var signUpTokenBytes = Encoding.UTF8.GetBytes(signUpTokenJson);
            var base64SignUpToken = Convert.ToBase64String(signUpTokenBytes);
            var rawToken = $"{base64SignUpToken}:{nonce}";
            var tokenBytes = Encoding.UTF8.GetBytes(rawToken);
            var token = Convert.ToBase64String(tokenBytes);

            return token;
        }

        public async Task<SignUpTokenDto> ParseTokenAsync(string token)
        {
            var tokenBytes = Convert.FromBase64String(token);
            var rawToken = Encoding.UTF8.GetString(tokenBytes);
            var tokenArray = rawToken.Split(':');

            if (tokenArray.Length == 2)
            {
                var base64SignUpToken = tokenArray[0];
                var nonce = tokenArray[1];
                var signUpTokenBytes = Convert.FromBase64String(base64SignUpToken);
                var signUpTokenJson = Encoding.UTF8.GetString(signUpTokenBytes);
                var signUpToken = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<SignUpTokenDto>(signUpTokenJson));

                if (signUpToken.Nonce == nonce)
                {
                    if (DateTime.UtcNow <= signUpToken.UtcDateIssued.AddHours(72))
                    {
                        return signUpToken;
                    }
                }
            }

            return null;
        }

        public async Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds)
        {
            var signUpToken = await this.CreateTokenAsync(propertyIds);
            var request = new RestRequest("{domain}/messages", Method.POST);
            request.AddParameter("domain", "mailgun.ownapt.com", ParameterType.UrlSegment);
            request.AddParameter("from", "OwnApt <support@ownapt.com>");
            request.AddParameter("to", email);
            request.AddParameter("subject", $"Welcome to Ownapt {name}!");
            request.AddParameter("html", string.Format(emailBody, signUpToken));

            try
            {
                return await Task.Factory.StartNew(() => (restClient.Execute(request)));
            }
            catch (Exception e)
            {
                ExceptionUtility.HandleException(e, this.logger, LogLevel.Error);
                throw;
            }
        }

        public async Task<bool> ValidateTokenAsync(string stringToken)
        {
            var signUpToken = await this.ParseTokenAsync(stringToken);
            return signUpToken != null;
        }

        #endregion Public Methods
    }
}

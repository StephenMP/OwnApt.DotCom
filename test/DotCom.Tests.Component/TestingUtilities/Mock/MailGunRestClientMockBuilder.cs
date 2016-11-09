using System.Net;
using Moq;
using OwnApt.DotCom.Domain.Service;
using RestSharp;

namespace DotCom.Tests.Component.TestingUtilities.Mock
{
    public class MailGunRestClientMockBuilder : MockBuilder<IMailGunRestClient>
    {
        #region Public Methods

        public static MailGunRestClientMockBuilder New() => new MailGunRestClientMockBuilder();

        private MailGunRestClientMockBuilder()
        {
            this.ExecuteAny(HttpStatusCode.OK);
        }

        public MailGunRestClientMockBuilder ExecuteAny(HttpStatusCode returnStatusCode)
        {
            this.Mock.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(new RestResponse { StatusCode = returnStatusCode });

            return this;
        }

        #endregion Public Methods
    }
}

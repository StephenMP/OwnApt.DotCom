using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using OwnApt.DotCom.Domain.Service;
using RestSharp;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class MailGunRestClientMockBuilder : MockBuilder<IMailGunRestClient>
    {
        public static MailGunRestClientMockBuilder NewBuilder() => new MailGunRestClientMockBuilder();

        public MailGunRestClientMockBuilder ExecuteAny(HttpStatusCode returnStatusCode)
        {
            this.Mock.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(new RestResponse { StatusCode = returnStatusCode });

            return this;
        }
    }
}

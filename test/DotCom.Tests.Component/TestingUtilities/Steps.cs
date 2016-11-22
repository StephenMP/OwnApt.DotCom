using System;
using System.Threading.Tasks;
using AutoMapper;
using DotCom.Tests.Component.TestingUtilities.Mock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.DotCom.AppStart;
using OwnApt.DotCom.Settings;
using OwnApt.RestfulProxy.Client;
using OwnApt.RestfulProxy.Interface;
using Xunit;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class Steps
    {
        #region Internal Fields

        internal Func<Task> asyncActionToExecute;
        internal ILoggerFactory loggerFactory;
        internal IMapper mapper;
        internal IProxy proxy;
        internal IOptions<ServiceUris> serviceUriOptions;
        internal ServiceUris serviceUris;

        #endregion Internal Fields

        #region Public Methods

        public void GivenIHaveAMockedProxy<TRequestDto, TResponseDto>(bool isSuccessfulStatusCode, TResponseDto responseDto = null) where TRequestDto : class where TResponseDto : class
        {
            var mockedProxyResponse = new ProxyResponse<TResponseDto>
            {
                IsSuccessfulStatusCode = isSuccessfulStatusCode,
                ResponseDto = responseDto
            };

            this.proxy = ProxyMockBuilder
                            .New()
                            .InvokeAsyncAny<TRequestDto, TResponseDto>(mockedProxyResponse)
                            .Build();
        }

        public void GivenIHaveMockedSystemAndThirdPartyObjects()
        {
            this.mapper = OwnAptStartup.BuildAutoMapper();

            this.serviceUris = new ServiceUris { ApiBaseUri = "http://this.is/a/test" };
            this.serviceUriOptions = OptionsMockBuilder<ServiceUris>.New()
                                                                    .Value(this.serviceUris)
                                                                    .Build();

            this.loggerFactory = LoggerFactoryMockBuilder.New()
                                                         .Build();
        }

        public async Task ThenICanVerifyIThrowAsync<T>(string errorMessage = null) where T : Exception
        {
            var exception = await Assert.ThrowsAsync<T>(this.asyncActionToExecute);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                Assert.Contains(errorMessage, exception.Message);
            }
        }

        #endregion Public Methods
    }
}

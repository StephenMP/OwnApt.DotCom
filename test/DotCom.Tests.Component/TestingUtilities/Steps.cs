using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        protected IMapper mapper;
        protected IOptions<ServiceUris> serviceUriOptions;
        protected ILoggerFactory loggerFactory;
        protected ServiceUris serviceUris;
        protected IProxy proxy;
        protected Func<Task> asyncActionToExecute;

        public async Task ThenICanVerifyIThrowAsync<T>(string errorMessage = null) where T : Exception
        {
            var exception = await Assert.ThrowsAsync<T>(this.asyncActionToExecute);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                Assert.Contains(errorMessage, exception.Message);
            }
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
    }
}

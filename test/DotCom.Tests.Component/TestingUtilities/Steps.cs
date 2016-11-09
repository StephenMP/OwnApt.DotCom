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
    public class Given
    {
        #region Protected Fields

        protected readonly Steps baseSteps;

        #endregion Protected Fields

        #region Public Constructors

        public Given(Steps steps)
        {
            this.baseSteps = steps;
        }

        #endregion Public Constructors

        #region Public Methods

        public void IHaveAMockedProxy<TRequestDto, TResponseDto>(bool isSuccessfulStatusCode, TResponseDto responseDto = null) where TRequestDto : class where TResponseDto : class
        {
            var mockedProxyResponse = new ProxyResponse<TResponseDto>
            {
                IsSuccessfulStatusCode = isSuccessfulStatusCode,
                ResponseDto = responseDto
            };

            this.baseSteps.proxy = ProxyMockBuilder
                            .New()
                            .InvokeAsyncAny<TRequestDto, TResponseDto>(mockedProxyResponse)
                            .Build();
        }

        public void IHaveMockedSystemAndThirdPartyObjects()
        {
            this.baseSteps.mapper = OwnAptStartup.BuildAutoMapper();

            this.baseSteps.serviceUris = new ServiceUris { ApiBaseUri = "http://this.is/a/test" };
            this.baseSteps.serviceUriOptions = OptionsMockBuilder<ServiceUris>.New()
                                                                    .Value(this.baseSteps.serviceUris)
                                                                    .Build();

            this.baseSteps.loggerFactory = LoggerFactoryMockBuilder.New()
                                                         .Build();
        }

        #endregion Public Methods
    }

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
    }

    public class Then
    {
        #region Protected Fields

        protected readonly Steps baseSteps;

        #endregion Protected Fields

        #region Public Constructors

        public Then(Steps steps)
        {
            this.baseSteps = steps;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task ICanVerifyIThrowAsync<T>(string errorMessage = null) where T : Exception
        {
            var exception = await Assert.ThrowsAsync<T>(this.baseSteps.asyncActionToExecute);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                Assert.Contains(errorMessage, exception.Message);
            }
        }

        #endregion Public Methods
    }

    public class When
    {
        #region Protected Fields

        protected readonly Steps baseSteps;

        #endregion Protected Fields

        #region Public Constructors

        public When(Steps steps)
        {
            this.baseSteps = steps;
        }

        #endregion Public Constructors
    }
}

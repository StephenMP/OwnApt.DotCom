using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using OwnApt.RestfulProxy.Client;
using OwnApt.RestfulProxy.Interface;

namespace DotCom.Tests.Component.TestingUtilities.Mock
{
    public class ProxyMockBuilder : MockBuilder<IProxy>
    {
        #region Public Methods

        public static ProxyMockBuilder New() => new ProxyMockBuilder();

        private ProxyMockBuilder()
        {
            var response = new ProxyResponse<Missing>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessfulStatusCode = true
            };

            this.InvokeAsyncAny<Missing, Missing>(response);
        }

        public ProxyMockBuilder InvokeAsyncAny<TRequestDto, TResponseDto>(IProxyResponse<TResponseDto> response)
        {
            this.Mock.Setup(m => m.InvokeAsync(It.IsAny<IProxyRequest<TRequestDto, TResponseDto>>())).Returns(Task.FromResult(response));

            return this;
        }

        #endregion Public Methods
    }
}

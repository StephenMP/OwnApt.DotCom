using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using OwnApt.RestfulProxy.Interface;

namespace DotCom.Tests.Component.TestingUtilities
{
    public class ProxyMockBuilder : MockBuilder<IProxy>
    {
        public static ProxyMockBuilder New() => new ProxyMockBuilder();

        public ProxyMockBuilder InvokeAsyncAny<TRequestDto, TResponseDto>(IProxyResponse<TResponseDto> response)
        {
            this.Mock.Setup(m => m.InvokeAsync(It.IsAny<IProxyRequest<TRequestDto, TResponseDto>>())).Returns(Task.FromResult(response));

            return this;
        }
    }
}

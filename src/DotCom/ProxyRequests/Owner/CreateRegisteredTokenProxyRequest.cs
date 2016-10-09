using System;
using System.Collections.Generic;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Settings;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;

namespace OwnApt.DotCom.ProxyRequests.Owner
{
    public class CreateRegisteredTokenProxyRequest : IProxyRequest<RegisteredTokenModel, RegisteredTokenModel>
    {
        #region Public Constructors

        public CreateRegisteredTokenProxyRequest(ServiceUris serviceUris, RegisteredTokenModel requestDto)
        {
            this.RequestUri = new Uri($"{serviceUris.ApiBaseUri.TrimEnd('/')}/api/v1/owner/signup/token/register");
            this.HttpRequestMethod = HttpRequestMethod.Post;
            this.RequestDto = requestDto;
            this.Headers = new Dictionary<string, IEnumerable<string>>
            {
                { "Accept", new string[] { "application/json" } }
            };
        }

        #endregion Public Constructors

        #region Public Properties

        public IDictionary<string, IEnumerable<string>> Headers { get; }
        public HttpRequestMethod HttpRequestMethod { get; }
        public RegisteredTokenModel RequestDto { get; }
        public Uri RequestUri { get; }

        #endregion Public Properties
    }
}

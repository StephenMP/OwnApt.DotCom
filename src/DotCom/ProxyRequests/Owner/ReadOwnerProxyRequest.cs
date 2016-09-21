using OwnApt.Api.Contract.Model;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OwnApt.DotCom.ProxyRequests.Owner
{
    public class ReadOwnerProxyRequest : IProxyRequest<Missing, OwnerModel>
    {
        #region Public Constructors

        public ReadOwnerProxyRequest(string baseUri, string ownerId)
        {
            this.RequestUri = new Uri($"{baseUri.TrimEnd('/')}/api/v1/owner/{ownerId}");
            this.HttpRequestMethod = HttpRequestMethod.Get;
            this.Headers = new Dictionary<string, IEnumerable<string>>
            {
                { "Accept", new string[] { "application/json" } }
            };
        }

        #endregion Public Constructors

        #region Public Properties

        public IDictionary<string, IEnumerable<string>> Headers { get; }
        public HttpRequestMethod HttpRequestMethod { get; }
        public Missing RequestDto { get; }
        public Uri RequestUri { get; }

        #endregion Public Properties
    }
}

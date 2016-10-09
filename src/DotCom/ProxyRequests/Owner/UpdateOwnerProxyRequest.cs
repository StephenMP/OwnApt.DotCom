using System;
using System.Collections.Generic;
using System.Reflection;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Settings;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;

namespace OwnApt.DotCom.ProxyRequests.Owner
{
    public class UpdateOwnerProxyRequest : IProxyRequest<OwnerModel, Missing>
    {
        #region Public Constructors

        public UpdateOwnerProxyRequest(ServiceUris serviceUris, OwnerModel ownerModel)
        {
            this.RequestUri = new Uri($"{serviceUris.ApiBaseUri.Trim('/')}/api/v1/owner");
            this.HttpRequestMethod = HttpRequestMethod.Put;
            this.RequestDto = ownerModel;
            this.Headers = new Dictionary<string, IEnumerable<string>>
            {
                { "Accept", new string[] { "application/json" } }
            };
        }

        #endregion Public Constructors

        #region Public Properties

        public IDictionary<string, IEnumerable<string>> Headers { get; }
        public HttpRequestMethod HttpRequestMethod { get; }
        public OwnerModel RequestDto { get; }
        public Uri RequestUri { get; }

        #endregion Public Properties
    }
}

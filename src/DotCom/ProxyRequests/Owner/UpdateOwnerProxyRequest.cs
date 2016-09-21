using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Dto.Account;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OwnApt.DotCom.ProxyRequests.Owner
{
    public class UpdateOwnerProxyRequest : IProxyRequest<OwnerModel, Missing>
    {
        #region Constructors

        public UpdateOwnerProxyRequest(string apiBaseUri, OwnerModel ownerModel)
        {
            this.RequestUri = new Uri($"{apiBaseUri.Trim('/')}/api/v1/owner");
            this.HttpRequestMethod = HttpRequestMethod.Put;
            this.RequestDto = ownerModel;
            this.Headers = new Dictionary<string, IEnumerable<string>>
            {
                { "Accept", new string[] { "application/json" } }
            };
        }

        #endregion Constructors

        #region Properties

        public IDictionary<string, IEnumerable<string>> Headers { get; }
        public HttpRequestMethod HttpRequestMethod { get; }
        public OwnerModel RequestDto { get; }
        public Uri RequestUri { get; }

        #endregion Properties
    }
}

using OwnApt.Api.Contract.Model;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OwnApt.DotCom.ProxyRequests.Lease
{
    public class ReadLeaseTermByPropertyIdProxyRequest : IProxyRequest<Missing, LeaseTermModel>
    {
        #region Public Constructors

        public ReadLeaseTermByPropertyIdProxyRequest(string baseUri, string propertyId)
        {
            this.RequestUri = new Uri($"{baseUri.TrimEnd('/')}/api/v1/lease/property/{propertyId}");
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

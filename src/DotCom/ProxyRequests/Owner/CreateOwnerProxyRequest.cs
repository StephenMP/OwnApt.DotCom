using OwnApt.Api.Contract.Model;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OwnApt.RestfulProxy.Domain.Enum;

namespace OwnApt.DotCom.ProxyRequests.Owner
{
    public class CreateOwnerProxyRequest : IProxyRequest<OwnerModel, OwnerModel>
    {
        public IDictionary<string, IEnumerable<string>> Headers { get; }
        public HttpRequestMethod HttpRequestMethod { get; }
        public OwnerModel RequestDto { get; }
        public Uri RequestUri { get; }

        public CreateOwnerProxyRequest(string baseUri, OwnerModel ownerModel)
        {
            this.RequestUri = new Uri($"{baseUri.TrimEnd('/')}/api/v1/owner");
            this.HttpRequestMethod = HttpRequestMethod.Post;
            this.RequestDto = ownerModel;
            this.Headers = new Dictionary<string, IEnumerable<string>>
            {
                { "Accept", new string[] { "application/json" } }
            };
        }
    }
}

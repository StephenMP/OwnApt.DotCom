using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.DotCom.Domain.Settings;
using System.Text;
using OwnApt.Api.Contract.Model;

namespace OwnApt.DotCom.ProxyRequests.Property
{
    public class ReadPropertiesProxyRequest : IProxyRequest<Missing, PropertyModel[]>
    {
        #region Public Properties

        public IDictionary<string, IEnumerable<string>> Headers { get; }
        public HttpRequestMethod HttpRequestMethod { get; }
        public Missing RequestDto { get; }
        public Uri RequestUri { get; }

        #endregion Public Properties

        public ReadPropertiesProxyRequest(ServiceUriSettings serviceUris, IEnumerable<string> propertyIds)
        {
            var queryParams = this.BuildQueryParams(propertyIds);
            this.RequestUri = new Uri($"{serviceUris.ApiBaseUri.TrimEnd('/')}/api/v1/property?{queryParams}");
            this.HttpRequestMethod = HttpRequestMethod.Get;
            this.Headers = new Dictionary<string, IEnumerable<string>>
            {
                { "Accept", new string[] { "application/json" } }
            };
        }

        private object BuildQueryParams(IEnumerable<string> propertyIds)
        {
            var builder = new StringBuilder();

            foreach(var id in propertyIds)
            {
                builder.Append($"propertyIds={id}&");
            }

            return builder.ToString().TrimEnd('&');
        }
    }
}

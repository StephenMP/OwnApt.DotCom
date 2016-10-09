using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Settings;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;

namespace OwnApt.DotCom.ProxyRequests.Property
{
    public class ReadPropertiesProxyRequest : IProxyRequest<Missing, PropertyModel[]>
    {
        #region Public Constructors

        public ReadPropertiesProxyRequest(ServiceUris serviceUris, IEnumerable<string> propertyIds)
        {
            var queryParams = this.BuildQueryParams(propertyIds);
            this.RequestUri = new Uri($"{serviceUris.ApiBaseUri.TrimEnd('/')}/api/v1/property?{queryParams}");
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

        #region Private Methods

        private object BuildQueryParams(IEnumerable<string> propertyIds)
        {
            var builder = new StringBuilder();

            foreach (var id in propertyIds)
            {
                builder.Append($"propertyIds={id}&");
            }

            return builder.ToString().TrimEnd('&');
        }

        #endregion Private Methods
    }
}

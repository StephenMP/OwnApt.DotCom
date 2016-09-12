using OwnApt.Api.Contract.Dto;
using OwnApt.DotCom.Dto.Account;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OwnApt.DotCom.ProxyRequests
{
    public class MapOwnerToPropertiesProxyRequest : IProxyRequest<MapOwnerToPropertiesDto, Missing>
    {
        #region Constructors

        public MapOwnerToPropertiesProxyRequest(string apiBaseUri, string ownerId, SignUpTokenDto signUpToken)
        {
            this.RequestUri = new Uri($"{apiBaseUri.Trim('/')}/api/v1/property/owner/addToProperties");
            this.HttpRequestMethod = HttpRequestMethod.Post;
            this.RequestDto = new MapOwnerToPropertiesDto
            {
                OwnerId = ownerId,
                PropertyIds = signUpToken.PropertyIds
            };
            this.Headers = new Dictionary<string, IEnumerable<string>>
            {
                { "Accept", new string[] { "application/json" } }
            };
        }

        #endregion Constructors

        #region Properties

        public IDictionary<string, IEnumerable<string>> Headers { get; }

        public HttpRequestMethod HttpRequestMethod { get; }

        public MapOwnerToPropertiesDto RequestDto { get; }

        public Uri RequestUri { get; }

        #endregion Properties
    }
}

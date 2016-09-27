﻿using OwnApt.Api.Contract.Model;
using OwnApt.Common.Security;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OwnApt.DotCom.ProxyRequests.Owner
{
    public class ReadRegisteredTokenProxyRequest : IProxyRequest<RegisteredTokenModel, RegisteredTokenModel>
    {
        #region Public Constructors

        public ReadRegisteredTokenProxyRequest(string baseUri, string token)
        {
            this.RequestUri = new Uri($"{baseUri.TrimEnd('/')}/api/v1/owner/signup/token");
            this.RequestDto = new RegisteredTokenModel { Token = token };
            this.HttpRequestMethod = HttpRequestMethod.Post;
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
﻿using OwnApt.Api.Contract.Model;
using OwnApt.RestfulProxy.Domain.Enum;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;

namespace OwnApt.DotCom.ProxyRequests.Owner
{
    public class CreateOwnerProxyRequest : IProxyRequest<OwnerModel, OwnerModel>
    {
        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public IDictionary<string, IEnumerable<string>> Headers { get; }
        public HttpRequestMethod HttpRequestMethod { get; }
        public OwnerModel RequestDto { get; }
        public Uri RequestUri { get; }

        #endregion Public Properties
    }
}
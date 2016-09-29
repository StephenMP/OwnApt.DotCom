using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.ProxyRequests.Lease;
using OwnApt.DotCom.ProxyRequests.Owner;
using OwnApt.DotCom.ProxyRequests.Property;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Service
{
    public interface IOwnerDomainService
    {
        Task<LeaseTermModel> ReadLeaseTermByPropertyId(string propertyId);
        Task<OwnerModel> ReadOwnerAsync(string ownerId);
        Task<PropertyModel> ReadPropertyAsync(string propertyId);
        Task<PropertyModel[]> ReadPropertiesAsync(IEnumerable<string> propertyIds);
    }

    public class OwnerDomainService : IOwnerDomainService
    {
        private readonly ILogger<OwnerDomainService> logger;
        private readonly IProxy proxy;
        private readonly ServiceUriSettings serviceUris;

        public OwnerDomainService
        (
            IProxy proxy,
            IOptions<ServiceUriSettings> serviceUris,
            ILoggerFactory loggerFactory
        )
        {
            this.proxy = proxy;
            this.serviceUris = serviceUris.Value;
            this.logger = loggerFactory.CreateLogger<OwnerDomainService>();
        }

        public async Task<LeaseTermModel> ReadLeaseTermByPropertyId(string propertyId)
        {
            var readLeaseTermByPropertyIdRequest = new ReadLeaseTermByPropertyIdProxyRequest(this.serviceUris.ApiBaseUri, propertyId);
            var readLeaseTermByPropertyIdResponse = await this.proxy.InvokeAsync(readLeaseTermByPropertyIdRequest);

            if (readLeaseTermByPropertyIdResponse.IsSuccessfulStatusCode)
            {
                return readLeaseTermByPropertyIdResponse.ResponseDto;
            }

            throw ExceptionUtility.RaiseException(readLeaseTermByPropertyIdResponse, this.logger);
        }

        public async Task<OwnerModel> ReadOwnerAsync(string ownerId)
        {
            var readOwnerRequest = new ReadOwnerProxyRequest(this.serviceUris, ownerId);
            var readOwnerResponse = await this.proxy.InvokeAsync(readOwnerRequest);
            if (readOwnerResponse.IsSuccessfulStatusCode)
            {
                return readOwnerResponse.ResponseDto;
            }

            throw ExceptionUtility.RaiseException(readOwnerResponse, this.logger);
        }

        public async Task<PropertyModel[]> ReadPropertiesAsync(IEnumerable<string> propertyIds)
        {
            var readPropertiesRequest = new ReadPropertiesProxyRequest(this.serviceUris, propertyIds);
            var readPropertiesResponse = await this.proxy.InvokeAsync(readPropertiesRequest);
            if (readPropertiesResponse.IsSuccessfulStatusCode)
            {
                return readPropertiesResponse.ResponseDto;
            }

            throw ExceptionUtility.RaiseException(readPropertiesResponse, this.logger);
        }

        public async Task<PropertyModel> ReadPropertyAsync(string propertyId)
        {
            var readPropertyRequest = new ReadPropertyProxyRequest(this.serviceUris, propertyId);
            var readPropertyResponse = await this.proxy.InvokeAsync(readPropertyRequest);
            if (readPropertyResponse.IsSuccessfulStatusCode)
            {
                return readPropertyResponse.ResponseDto;
            }

            throw ExceptionUtility.RaiseException(readPropertyResponse, this.logger);
        }
    }
}

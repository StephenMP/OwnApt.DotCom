using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.ProxyRequests.Lease;
using OwnApt.DotCom.ProxyRequests.Owner;
using OwnApt.DotCom.ProxyRequests.Property;
using OwnApt.DotCom.Settings;
using OwnApt.RestfulProxy.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Service
{
    public interface IOwnerDomainService
    {
        #region Public Methods

        Task<LeaseTermModel> ReadLeaseTermByPropertyId(string propertyId);

        Task<OwnerModel> ReadOwnerAsync(string ownerId);

        Task<PropertyModel[]> ReadPropertiesAsync(IEnumerable<string> propertyIds);

        Task<PropertyModel> ReadPropertyAsync(string propertyId);

        #endregion Public Methods
    }

    public class OwnerDomainService : IOwnerDomainService
    {
        #region Private Fields

        private readonly ILogger<OwnerDomainService> logger;
        private readonly IProxy proxy;
        private readonly ServiceUris serviceUris;

        #endregion Private Fields

        #region Public Constructors

        public OwnerDomainService
        (
            IProxy proxy,
            IOptions<ServiceUris> serviceUris,
            ILoggerFactory loggerFactory
        )
        {
            this.proxy = proxy;
            this.serviceUris = serviceUris.Value;
            this.logger = loggerFactory.CreateLogger<OwnerDomainService>();
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}

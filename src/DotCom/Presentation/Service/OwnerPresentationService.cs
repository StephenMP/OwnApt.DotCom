using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Exceptions;
using OwnApt.DotCom.Domain.Settings;
using OwnApt.DotCom.Model.Owner;
using OwnApt.DotCom.ProxyRequests.Lease;
using OwnApt.DotCom.ProxyRequests.Owner;
using OwnApt.DotCom.ProxyRequests.Property;
using OwnApt.RestfulProxy.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IOwnerPresentationService
    {
        #region Public Methods

        Task<OwnerIndexModel> BuildIndexModelAsync(string ownerId);

        Task<LeaseTermModel> ReadLeaseTermByPropertyId(string propertyId);

        Task<OwnerModel> ReadOwnerAsync(string ownerId);

        Task<PropertyModel> ReadProperty(string propertyId);

        #endregion Public Methods
    }

    public class OwnerPresentationService : IOwnerPresentationService
    {
        #region Private Fields

        private readonly ILogger<OwnerPresentationService> logger;
        private readonly IProxy proxy;
        private readonly ServiceUriSettings serviceUris;
        private readonly IMapper mapper;

        #endregion Private Fields

        #region Public Constructors

        public OwnerPresentationService
        (
            IProxy proxy,
            IOptions<ServiceUriSettings> serviceUris,
            ILoggerFactory loggerFactory,
            IMapper mapper
        )
        {
            this.proxy = proxy;
            this.serviceUris = serviceUris.Value;
            this.logger = loggerFactory.CreateLogger<OwnerPresentationService>();
            this.mapper = mapper;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<OwnerIndexModel> BuildIndexModelAsync(string ownerId)
        {
            var model = new OwnerIndexModel { OwnerId = ownerId };

            var owner = await this.ReadOwnerAsync(ownerId);
            var properties = new List<PropertyModel>();
            var leaseTermsByPropertyId = new Dictionary<string, LeaseTermViewModel>();

            foreach (var ownerPropertyId in owner.PropertyIds)
            {
                var property = await this.ReadProperty(ownerPropertyId);
                properties.Add(property);
            }

            foreach (var property in properties)
            {
                var leaseTerm = await this.ReadLeaseTermByPropertyId(property.Id);
                var leaseTermView = mapper.Map<LeaseTermViewModel>(leaseTerm);
                leaseTermsByPropertyId.Add(property.Id, leaseTermView);
            }

            model.LeaseTermsByPropertyId = leaseTermsByPropertyId;
            model.Properties = properties;

            return model;
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
            var readOwnerRequest = new ReadOwnerProxyRequest(this.serviceUris.ApiBaseUri, ownerId);
            var readOwnerResponse = await this.proxy.InvokeAsync(readOwnerRequest);
            if (readOwnerResponse.IsSuccessfulStatusCode)
            {
                return readOwnerResponse.ResponseDto;
            }

            throw ExceptionUtility.RaiseException(readOwnerResponse, this.logger);
        }

        public async Task<PropertyModel> ReadProperty(string propertyId)
        {
            var readPropertyRequest = new ReadPropertyProxyRequest(serviceUris.ApiBaseUri, propertyId);
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

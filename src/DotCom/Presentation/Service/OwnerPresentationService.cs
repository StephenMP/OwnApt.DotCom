using AutoMapper;
using Microsoft.Extensions.Logging;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Domain.Service;
using OwnApt.DotCom.Model.Owner;
using OwnApt.RestfulProxy.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Presentation.Service
{
    public interface IOwnerPresentationService
    {
        #region Public Methods

        Task<OwnerIndexViewModel> BuildIndexModelAsync(string ownerId);

        Task<OwnerProfileViewModel> BuildProfileModelAsync(string ownerId);

        Task<LeaseTermModel> ReadLeaseTermByPropertyId(string propertyId);

        Task<OwnerModel> ReadOwnerAsync(string ownerId);

        Task<PropertyModel> ReadPropertyAsync(string propertyId);

        #endregion Public Methods
    }

    public class OwnerPresentationService : IOwnerPresentationService
    {
        #region Private Fields

        private readonly ILogger<OwnerPresentationService> logger;
        private readonly IMapper mapper;
        private readonly IOwnerDomainService ownerDomainService;
        private readonly IProxy proxy;

        #endregion Private Fields

        #region Public Constructors

        public OwnerPresentationService
        (
            IOwnerDomainService ownerDomainService,
            IProxy proxy,
            ILoggerFactory loggerFactory,
            IMapper mapper
        )
        {
            this.ownerDomainService = ownerDomainService;
            this.proxy = proxy;
            this.logger = loggerFactory.CreateLogger<OwnerPresentationService>();
            this.mapper = mapper;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<OwnerIndexViewModel> BuildIndexModelAsync(string ownerId)
        {
            var model = new OwnerIndexViewModel { OwnerId = ownerId };
            var owner = await this.ownerDomainService.ReadOwnerAsync(ownerId);
            var properties = await this.ownerDomainService.ReadPropertiesAsync(owner.PropertyIds);
            var leaseTermsByPropertyId = new Dictionary<string, LeaseTermViewModel>();

            foreach (var property in properties)
            {
                var leaseTerm = await this.ReadLeaseTermByPropertyId(property.Id);
                var leaseTermView = mapper.Map<LeaseTermViewModel>(leaseTerm);
                leaseTermsByPropertyId.Add(property.Id, leaseTermView);
            }

            model.LeaseTermsByPropertyId = leaseTermsByPropertyId;
            model.Properties.AddRange(properties);

            return model;
        }

        public async Task<OwnerProfileViewModel> BuildProfileModelAsync(string ownerId)
        {
            var ownerModel = await this.ownerDomainService.ReadOwnerAsync(ownerId);
            var ownerProfileViewModel = this.mapper.Map<OwnerProfileViewModel>(ownerModel);
            return ownerProfileViewModel;
        }

        public async Task<LeaseTermModel> ReadLeaseTermByPropertyId(string propertyId)
        {
            return await this.ownerDomainService.ReadLeaseTermByPropertyId(propertyId);
        }

        public async Task<OwnerModel> ReadOwnerAsync(string ownerId)
        {
            return await this.ownerDomainService.ReadOwnerAsync(ownerId);
        }

        public async Task<PropertyModel> ReadPropertyAsync(string propertyId)
        {
            return await this.ownerDomainService.ReadPropertyAsync(propertyId);
        }

        #endregion Public Methods
    }
}

using System.Linq;
using AutoMapper;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Model.Owner;

namespace OwnApt.DotCom.Mapping
{
    public class OwnerProfile : Profile
    {
        #region Public Constructors

        public OwnerProfile()
        {
            ConfigureAccountViewModel();
            ConfigureLeaseTermViewModel();
            ConfigureOwnerViewModel();
            ConfigurePropertyViewModel();
        }

        #endregion Public Constructors

        #region Private Methods

        private void ConfigureAccountViewModel()
        {
            CreateMap<AddressModel, AddressViewModel>()
                .ForMember(dest => dest.ZipBase, options => options.MapFrom(src => src.Zip.Base))
                .ForMember(dest => dest.ZipExtension, options => options.MapFrom(src => src.Zip.Extension));
        }

        private void ConfigureLeaseTermViewModel()
        {
            CreateMap<LeaseTermModel, LeaseTermViewModel>()
                .ForMember(dest => dest.CurrentPeriod, options => options.MapFrom(src => src.LeasePeriods.FirstOrDefault(lp => lp.Period == src.LeasePeriods.Max(p => p.Period))));
        }

        private void ConfigureOwnerViewModel()
        {
            CreateMap<OwnerModel, OwnerProfileViewModel>();
        }

        private void ConfigurePropertyViewModel()
        {
            CreateMap<PropertyModel, PropertyViewModel>();
        }

        #endregion Private Methods
    }
}

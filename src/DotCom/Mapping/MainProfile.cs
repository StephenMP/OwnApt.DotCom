using AutoMapper;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Dto.Account;
using OwnApt.DotCom.Model.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Mapping
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            ConfigureSignUp();
            ConfigureLeaseTermViewModel();
            ConfigureOwnerViewModel();
        }

        private void ConfigureOwnerViewModel()
        {
            CreateMap<OwnerModel, OwnerProfileViewModel>();
            CreateMap<OwnerProfileViewModel, OwnerModel>();
        }

        private void ConfigureLeaseTermViewModel()
        {
            CreateMap<LeaseTermModel, LeaseTermViewModel>();
            CreateMap<LeaseTermViewModel, LeaseTermModel>();
        }

        private void ConfigureSignUp()
        {
            CreateMap<SignUpTokenDto, RegisteredTokenModel>();
            CreateMap<RegisteredTokenModel, SignUpTokenDto>();
        }
    }
}

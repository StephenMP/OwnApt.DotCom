﻿using AutoMapper;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Dto.Account;
using OwnApt.DotCom.Model.Owner;

namespace OwnApt.DotCom.Mapping
{
    public class MainProfile : Profile
    {
        #region Public Constructors

        public MainProfile()
        {
            ConfigureSignUp();
            ConfigureLeaseTermViewModel();
            ConfigureOwnerViewModel();
        }

        #endregion Public Constructors

        #region Private Methods

        private void ConfigureLeaseTermViewModel()
        {
            CreateMap<LeaseTermModel, LeaseTermViewModel>();
            CreateMap<LeaseTermViewModel, LeaseTermModel>();
        }

        private void ConfigureOwnerViewModel()
        {
            CreateMap<OwnerModel, OwnerProfileViewModel>();
            CreateMap<OwnerProfileViewModel, OwnerModel>();
        }

        private void ConfigureSignUp()
        {
            CreateMap<SignUpTokenDto, RegisteredTokenModel>();
            CreateMap<RegisteredTokenModel, SignUpTokenDto>();
        }

        #endregion Private Methods
    }
}

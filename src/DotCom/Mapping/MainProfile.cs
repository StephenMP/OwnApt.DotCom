using AutoMapper;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Dto.Account;
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
        }

        private void ConfigureSignUp()
        {
            CreateMap<SignUpTokenDto, RegisteredTokenModel>();
            CreateMap<RegisteredTokenModel, SignUpTokenDto>();
        }
    }
}

using AutoMapper;
using OwnApt.Api.Contract.Model;
using OwnApt.DotCom.Dto.Account;

namespace OwnApt.DotCom.Mapping
{
    public class AccountProfile : Profile
    {
        #region Public Constructors

        public AccountProfile()
        {
            ConfigureSignUp();
        }

        #endregion Public Constructors

        #region Private Methods

        private void ConfigureSignUp()
        {
            CreateMap<SignUpTokenDto, RegisteredTokenModel>();
            CreateMap<RegisteredTokenModel, SignUpTokenDto>();
        }

        #endregion Private Methods
    }
}

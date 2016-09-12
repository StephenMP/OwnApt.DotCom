using OwnApt.DotCom.Dto.Account;
using RestSharp;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Interface
{
    public interface ISignUpService
    {
        #region Methods

        Task<string> CreateTokenAsync(params string[] propertyIds);

        Task<SignUpTokenDto> ParseTokenAsync(string token);

        Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds);

        Task<bool> ValidateTokenAsync(string stringToken);

        #endregion Methods
    }
}

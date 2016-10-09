using System.Threading.Tasks;
using OwnApt.DotCom.Dto.Account;
using RestSharp;

namespace OwnApt.DotCom.Domain.Interface
{
    public interface ISignUpService
    {
        #region Public Methods

        Task<string> CreateTokenAsync(params string[] propertyIds);

        Task<SignUpTokenDto> ParseTokenAsync(string token);

        Task<IRestResponse> SendSignUpEmailAsync(string name, string email, string[] propertyIds);

        Task<bool> ValidateTokenAsync(string stringToken);

        #endregion Public Methods
    }
}

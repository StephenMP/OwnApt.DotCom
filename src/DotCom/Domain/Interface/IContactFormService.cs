using RestSharp;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Interface
{
    public interface IContactFormService
    {
        #region Public Methods

        Task<IRestResponse> SendEmailAsync(string name, string message);

        #endregion Public Methods
    }
}

using RestSharp;
using System.Threading.Tasks;

namespace DotCom.Services
{
    public interface IEmailService
    {
        Task<IRestResponse> SendEmailAsync(string name, string message);
    }
}

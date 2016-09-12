using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Interface
{
    public interface IClaimsService
    {
        #region Methods

        Task<string> GetUserEmailAsync(IEnumerable<Claim> userClaims);

        Task<string> GetUserIdAsync(IEnumerable<Claim> userClaims);

        #endregion Methods
    }
}

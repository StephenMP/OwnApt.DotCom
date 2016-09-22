using OwnApt.DotCom.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Domain.Service
{
    public class ClaimsService : IClaimsService
    {
        #region Public Methods

        public async Task<string> GetUserEmailAsync(IEnumerable<Claim> userClaims)
        {
            var userEmailClaim = userClaims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            var userEmail = userEmailClaim.Value;

            return await Task.FromResult(userEmail);
        }

        public async Task<string> GetUserIdAsync(IEnumerable<Claim> userClaims)
        {
            var userIdClaim = userClaims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var userId = userIdClaim.Value.Split('|')[1];

            return await Task.FromResult(userId);
        }

        #endregion Public Methods
    }
}

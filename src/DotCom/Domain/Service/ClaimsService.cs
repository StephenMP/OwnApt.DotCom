using System.Linq;
using System.Security.Claims;

namespace OwnApt.DotCom.Domain.Service
{
    public interface IClaimsService
    {
        #region Public Methods

        string GetUserEmail(ClaimsPrincipal user);

        string GetUserId(ClaimsPrincipal user);

        string GetUserImage(ClaimsPrincipal user);

        string GetUserName(ClaimsPrincipal user);

        #endregion Public Methods
    }

    public class ClaimsService : IClaimsService
    {
        #region Public Methods

        public string GetUserEmail(ClaimsPrincipal user)
        {
            var userEmail = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            return userEmail;
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var userId = userIdClaim?.Split('|')[1];
            return userId;
        }

        public string GetUserImage(ClaimsPrincipal user)
        {
            var profileImage = user.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            return profileImage;
        }

        public string GetUserName(ClaimsPrincipal user)
        {
            var userName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            return userName;
        }

        #endregion Public Methods
    }
}

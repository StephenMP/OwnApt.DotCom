using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Moq;
using OwnApt.DotCom.Domain.Service;

namespace DotCom.Tests.Component.TestingUtilities.Mock
{
    public class ClaimsServiceMockBuilder : MockBuilder<IClaimsService>
    {
        public static ClaimsServiceMockBuilder New() => new ClaimsServiceMockBuilder();

        private ClaimsServiceMockBuilder()
        {
            this.GetUserEmailAny("test");
            this.GetUserIdAny("test");
            this.GetUserImageAny("test");
            this.GetUserNameAny("test");
        }

        public ClaimsServiceMockBuilder GetUserEmailAny(string userEmail)
        {
            this.Mock.Setup(m => m.GetUserEmail(It.IsAny<ClaimsPrincipal>())).Returns(userEmail);
            return this;
        }

        public ClaimsServiceMockBuilder GetUserIdAny(string userId)
        {
            this.Mock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            return this;
        }

        public ClaimsServiceMockBuilder GetUserImageAny(string userImagePath)
        {
            this.Mock.Setup(m => m.GetUserImage(It.IsAny<ClaimsPrincipal>())).Returns(userImagePath);
            return this;
        }

        public ClaimsServiceMockBuilder GetUserNameAny(string userName)
        {
            this.Mock.Setup(m => m.GetUserName(It.IsAny<ClaimsPrincipal>())).Returns(userName);
            return this;
        }

        public ClaimsServiceMockBuilder GetUserEmail(ClaimsPrincipal user, string userEmail)
        {
            this.Mock.Setup(m => m.GetUserEmail(user)).Returns(userEmail);
            return this;
        }

        public ClaimsServiceMockBuilder GetUserId(ClaimsPrincipal user, string userId)
        {
            this.Mock.Setup(m => m.GetUserId(user)).Returns(userId);
            return this;
        }

        public ClaimsServiceMockBuilder GetUserImage(ClaimsPrincipal user, string userImage)
        {
            this.Mock.Setup(m => m.GetUserImage(user)).Returns(userImage);
            return this;
        }

        public ClaimsServiceMockBuilder GetUserName(ClaimsPrincipal user, string userName)
        {
            this.Mock.Setup(m => m.GetUserName(user)).Returns(userName);
            return this;
        }
    }
}

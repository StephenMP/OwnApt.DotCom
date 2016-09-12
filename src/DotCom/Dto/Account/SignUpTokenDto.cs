using System;

namespace OwnApt.DotCom.Dto.Account
{
    public class SignUpTokenDto
    {
        #region Properties

        public string Nonce { get; set; }
        public string[] PropertyIds { get; set; }
        public string SuppliedNonce { get; set; }
        public DateTime UtcDateIssued { get; set; }

        #endregion Properties
    }
}

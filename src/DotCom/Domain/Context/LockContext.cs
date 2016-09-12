namespace OwnApt.DotCom.Domain.Context
{
    public class LockContext
    {
        #region Properties

        public string CallbackUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
        public string Nonce { get; set; }
        public string State { get; set; }

        #endregion Properties
    }
}

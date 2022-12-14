namespace OwnApt.DotCom.Model.Account
{
    public class LockContextModel
    {
        #region Public Properties

        public string CallbackUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
        public string Nonce { get; set; }
        public string State { get; set; }

        #endregion Public Properties
    }
}

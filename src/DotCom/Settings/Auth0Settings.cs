﻿namespace OwnApt.DotCom.Domain.Settings
{
    public class Auth0Settings
    {
        #region Public Properties

        public string CallbackUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }

        #endregion Public Properties
    }
}

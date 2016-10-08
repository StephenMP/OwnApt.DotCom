using OwnApt.Api.Contract.Model;
using OwnApt.Common.Enums;
using System;
using System.Collections.Generic;

namespace OwnApt.DotCom.Model.Owner
{
    public class OwnerProfileViewModel
    {
        #region Public Properties

        public DateTime Birthdate { get; set; }
        public ContactModel Contact { get; set; }
        public ContactModel EmergencyContact { get; set; }
        public Gender Gender { get; set; }
        public string Id { get; set; }
        public NameModel Name { get; set; }
        public List<string> PropertyIds { get; set; }

        #endregion Public Properties
    }
}

using OwnApt.Api.Contract.Model;
using OwnApt.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Model.Owner
{
    public class OwnerProfileViewModel
    {
        public DateTime Birthdate { get; set; }
        public ContactModel Contact { get; set; }
        public ContactModel EmergencyContact { get; set; }
        public Gender Gender { get; set; }
        public string Id { get; set; }
        public NameModel Name { get; set; }
        public List<string> PropertyIds { get; set; }
    }
}

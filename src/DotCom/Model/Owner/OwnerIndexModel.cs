using OwnApt.Api.Contract.Model;
using System.Collections.Generic;

namespace OwnApt.DotCom.Model.Owner
{
    public class OwnerIndexModel
    {
        #region Public Properties

        public Dictionary<string, LeaseTermModel> LeaseTermsByPropertyId { get; set; }
        public string OwnerId { get; set; }
        public List<PropertyModel> Properties { get; set; }

        #endregion Public Properties
    }
}

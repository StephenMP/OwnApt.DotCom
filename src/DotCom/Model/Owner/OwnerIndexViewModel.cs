using System.Collections.Generic;

namespace OwnApt.DotCom.Model.Owner
{
    public class OwnerIndexViewModel
    {
        #region Public Constructors

        public OwnerIndexViewModel()
        {
            this.Properties = new List<PropertyViewModel>();
            this.LeaseTermsByPropertyId = new Dictionary<string, LeaseTermViewModel>();
        }

        #endregion Public Constructors

        #region Public Properties

        public Dictionary<string, LeaseTermViewModel> LeaseTermsByPropertyId { get; set; }
        public string OwnerId { get; set; }
        public List<PropertyViewModel> Properties { get; set; }

        #endregion Public Properties
    }
}

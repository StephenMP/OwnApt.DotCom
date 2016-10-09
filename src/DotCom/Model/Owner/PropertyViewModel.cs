using System;
using OwnApt.Api.Contract.Model;
using OwnApt.Common.Enums;

namespace OwnApt.DotCom.Model.Owner
{
    public class PropertyViewModel
    {
        #region Public Properties

        public AddressViewModel Address { get; set; }
        public FeaturesModel Features { get; set; }
        public string Id { get; set; }
        public Uri ImageUri { get; set; }
        public string PropertyDescription { get; set; }
        public PropertyType PropertyType { get; set; }

        #endregion Public Properties
    }
}

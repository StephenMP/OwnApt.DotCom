using OwnApt.Common.Enums;

namespace OwnApt.DotCom.Model.Owner
{
    public class AddressViewModel
    {
        #region Public Properties

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public State State { get; set; }

        public string ZipBase { get; set; }

        public string ZipExtension { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            var addressLine = string.IsNullOrWhiteSpace(Address2) ? Address1 : $"{Address1}, {Address2}";
            var zip = string.IsNullOrWhiteSpace(ZipExtension) ? ZipBase : $"{ZipBase}-{ZipExtension}";

            return $"{addressLine}, {City}, {State} {zip}";
        }

        #endregion Public Methods
    }
}

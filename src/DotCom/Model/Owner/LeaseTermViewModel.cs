using System;

namespace OwnApt.DotCom.Model.Owner
{
    public class LeaseTermViewModel
    {
        #region Public Properties

        public DateTime EndDate { get; set; }
        public string LeaseTermId { get; set; }
        public string PropertyId { get; set; }

        public int Remaining
        {
            get
            {
                if (DateTime.Now < this.EndDate)
                {
                    var months = Math.Abs(DateTime.Now.Month - this.EndDate.Month);
                    return months == 0 ? 12 : months;
                }
                else
                {
                    return 0;
                }
            }
        }

        public decimal Rent { get; set; }
        public DateTime StartDate { get; set; }
        public string Status => (DateTime.Now > this.StartDate && DateTime.Now < this.EndDate) ? "Occupied" : "Vacant";

        #endregion Public Properties
    }
}

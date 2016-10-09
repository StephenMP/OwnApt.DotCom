using System;
using OwnApt.Api.Contract.Model;
using OwnApt.Common.Enums;

namespace OwnApt.DotCom.Model.Owner
{
    public class LeaseTermViewModel
    {
        #region Public Fields

        public const string Green = "health-green";
        public const string Red = "health-red";
        public const string Yellow = "health-yellow";

        #endregion Public Fields

        #region Public Properties

        public LeasePeriodModel CurrentPeriod { get; set; }

        public DateTime CurrentPeriodDate
        {
            get
            {
                var year = Convert.ToInt32(this.CurrentPeriod.Period.Substring(0, 4));
                var month = Convert.ToInt32(this.CurrentPeriod.Period.Substring(4, 2));
                return new DateTime(year, month, 1);
            }
        }

        public DateTime EndDate { get; set; }

        public string Health
        {
            get
            {
                var paymentDueDays = (DateTime.Now - CurrentPeriodDate).TotalDays;

                if (CurrentPeriod.LeasePeriodStatus == LeasePeriodStatus.PaymentDue)
                {
                    if (paymentDueDays < 5)
                    {
                        this.HealthDescription = "Rent Payment is Due";
                        return Green;
                    }

                    if (paymentDueDays < 10)
                    {
                        this.HealthDescription = "Rent Payment is Late";
                        return Yellow;
                    }

                    this.HealthDescription = "Rent Payment is Delinquent";
                    return Red;
                }

                if (CurrentPeriod.LeasePeriodStatus == LeasePeriodStatus.PaymentReceived)
                {
                    if (paymentDueDays < 10)
                    {
                        this.HealthDescription = "Rent Payment Has Been Received and is Processing";
                        return Green;
                    }

                    if (paymentDueDays < 10)
                    {
                        this.HealthDescription = "Payment Distribution is Taking Longer Than Normal";
                        return Yellow;
                    }

                    this.HealthDescription = "We Haven't Distributed Rent Payment";
                    return Red;
                }

                if (this.IsActive)
                {
                    this.HealthDescription = "Rent is Current and Has Been Distributed";
                    return Green;
                }

                this.HealthDescription = "The Property is Vacant";
                return Red;
            }
        }

        public string HealthDescription { get; set; }
        public bool IsActive => (DateTime.Now > this.StartDate && DateTime.Now < this.EndDate);

        public string LeaseTermId { get; set; }

        public string PropertyId { get; set; }

        public decimal Rent { get; set; }

        public DateTime StartDate { get; set; }

        public string Status => this.IsActive ? "Occupied" : "Vacant";

        public int TermRemaining => this.IsActive ? ((this.EndDate.Year - DateTime.Now.Year) * 12) + this.EndDate.Month - DateTime.Now.Month : 0;

        #endregion Public Properties
    }
}

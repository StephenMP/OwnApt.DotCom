using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwnApt.DotCom.Model.Owner
{
    public class LeaseTermViewModel
    {

        public DateTime EndDate { get; set; }
        public string LeaseTermId { get; set; }
        public string PropertyId { get; set; }
        public decimal Rent { get; set; }
        public DateTime StartDate { get; set; }
        public string Status
        {
            get
            {
                if (DateTime.Now > StartDate && DateTime.Now < EndDate)
                {
                    return "Occupied";
                }
                else
                {
                    return "Vacant";
                }
            }
        }
        public int Remaining
        {
            get
            {
                if(DateTime.Now < EndDate)
                {
                    return DateTime.Now.Month - EndDate.Month;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}

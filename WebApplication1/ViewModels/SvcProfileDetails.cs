using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class SvcProfileDetails
    {
        public int SvcProfileID { get; set; }
        public int ProfileID { get; set; }
        public int SvcTypeID { get; set; }
        public string SvcName { get; set; }
        public System.DateTime BillingDate { get; set; }
        public string BillingMethod { get; set; }
        public Nullable<System.DateTime> SvcStartDate { get; set; }
        public Nullable<System.DateTime> SvcEndDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

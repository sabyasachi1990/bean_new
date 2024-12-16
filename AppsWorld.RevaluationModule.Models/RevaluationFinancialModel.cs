using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Models
{
    public class RevaluationFinancialModel
    {
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
    }
}

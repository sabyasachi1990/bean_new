using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Models
{
    public class DocumentResetModel
    {

        public string PeriodLockPassword { get; set; }
        public string Version { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        //public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        //public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public List<Guid> ClearingIds { get; set; }

    }

    //public class ClearingId
    //{
    //    public System.Guid Id { get; set; }
    //}
}

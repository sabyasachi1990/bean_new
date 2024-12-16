using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class DocumentResetModel
    {
        public System.Guid Id { get; set; }

        public System.Guid InvoiceId { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.DateTime> ResetDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public string Version { get; set; }
        public Nullable<DateTime> FinancialStartDate { get; set; }
        public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}

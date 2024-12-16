using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Models
{
    public class RevaluationLUs
    {
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string DocState { get; set; }
        public bool? IsRevaluation { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool? IsLocked { get; set; }
        public string Version { get; set; }
    }
}

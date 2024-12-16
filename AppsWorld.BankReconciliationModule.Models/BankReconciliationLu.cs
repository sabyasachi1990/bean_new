using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BankReconciliationModule.Models
{
    public class BankReconciliationLu
    {
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public long CompanyId { get; set; }
		//public List<LookUp<string>> ChartOfAccountLU { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
		//public List<LookUp<string>> AllChartOfAccountLU { get; set; }
    }
}

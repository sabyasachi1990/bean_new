using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BankWithdrawalModule.Models
{
    public class WithdrawalModelLU
    {
        public long CompanyId { get; set; }
        //public LookUpCategory<string> SegmentCategory1LU { get; set; }
        //public LookUpCategory<string> SegmentCategory2LU { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        //public List<LookUp<string>> ChartOfAccountLU { get; set; }
        public List<COALookup<string>> ChartOfAccountNewLU { get; set; }
        public LookUpCategory<string> ModeOfWithdrawLU { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public LookUpCategory<string> EntityTypeLU { get; set; }
    }
}

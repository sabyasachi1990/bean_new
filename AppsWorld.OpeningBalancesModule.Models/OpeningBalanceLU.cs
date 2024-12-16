using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class OpeningBalanceLU
    {
        public long CompanyId { get; set; }
       // public long ServiceCompanyId { get; set; }
        public string BaseCurrency { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        //public LookUpCategory<string> SegmentCategory1LU { get; set; }
        //public LookUpCategory<string> SegmentCategory2LU { get; set; }
        // public bool IsMultiCurrency { get; set; }
        public bool? IsLocked { get; set; }

    }
}

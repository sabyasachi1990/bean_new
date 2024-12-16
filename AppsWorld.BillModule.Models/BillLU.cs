using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
   public class BillLU
    {
        public long CompanyId { get; set; }

        //public LookUpCategory<string> SegmentCategory1LU { get; set; }
        //public LookUpCategory<string> SegmentCategory2LU { get; set; }

        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        //public LookUpCategory<string> NatureLU { get; set; }
		//public LookUpCategory<string> VendorTypeLU { get; set; }
		//public LookUpCategory<string> AllowableNonallowableLU { get; set; } 
        public List<LookUp<string>> TermsOfPaymentLU { get; set; }

        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }

    }
}

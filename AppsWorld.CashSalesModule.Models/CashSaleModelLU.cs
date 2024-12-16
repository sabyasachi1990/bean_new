using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.CashSalesModule.Models
{
    public class CashSaleModelLU 
    {
        public long CompanyId { get; set; }

       
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        public LookUpCategory<string> ModeOfReciptLU { get; set; }
        public LookUpCategory<string> AllowableNonallowableLU { get; set; }
        public LookUpCategory<string> SegmentCategory1LU { get; set; }
        public LookUpCategory<string> SegmentCategory2LU { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public List<LookUp<string>> CashAndBankCOALU { get; set; }
        
    }
}

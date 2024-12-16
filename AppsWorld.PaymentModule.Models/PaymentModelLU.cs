using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.PaymentModule.Models
{
    public class PaymentModelLU
    {
        public long CompanyId { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        //public LookUpCategory<string> NatureLU { get; set; }
        public List<string> NatureLU { get; set; }
        public LookUpCategory<string> ModeOfPaymentLU { get; set; }
        //public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        //public List<LookUp<string>> ChartOfAccountLU { get; set; }
        public List<COALookup<string>> AllChartOfAccountLU { get; set; }
    }
}

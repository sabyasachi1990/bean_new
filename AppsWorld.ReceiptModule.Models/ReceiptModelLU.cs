using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.ReceiptModule.Models
{
    public class ReceiptModelLU
    {
        public ReceiptModelLU()
        {
            this.ModeOfReceiptLU = new LookUpCategory<string>();
        }
        public long CompanyId { get; set; }

        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        public LookUpCategory<string> NatureLU { get; set; }
		public LookUpCategory<string> ModeOfReceiptLU { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
		public List<COALookup<string>> ChartOfAccountLU { get; set; }
		public List<COALookup<string>> AllChartOfAccountLU { get; set; }
    }
}

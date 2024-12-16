using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JournalTaxDetailsModel
    {
        public decimal? GrandDocDebitTotal { get; set; }
        public decimal? GrandDocCreditTotal { get; set; }
        public Nullable<decimal> GrandBaseDebitTotal { get; set; }
        public Nullable<decimal> GrandBaseCreditTotal { get; set; }
		public decimal? GrandDocTaxDebitTotal { get; set; }
		public decimal? GrandDocTaxCreditTotal { get; set; }
		public Nullable<decimal> GrandBaseTaxDebitTotal { get; set; }
		public Nullable<decimal> GrandBaseTaxCreditTotal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class TaxDetailsModel
    {
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public decimal? TaxDocDebit { get; set; }
        public decimal? TaxDocCredit { get; set; }
        public decimal? TaxBaseDebit { get; set; }
        public decimal? TaxBaseCredit { get; set; }
        public string TaxType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class DoubtfulDebtDetailModel
    {
        public string TaxCode { get; set; }
        public string TaxType { get; set; }
        public Nullable<double> TaxRate { get; set; }
    }
}

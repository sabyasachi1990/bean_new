using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class DebitNoteDetailModel
    {
        public string TaxIdCode { get; set; }
        public long? COAId { get; set; }
        public decimal? DocAmount { get; set; }
        public decimal? DocTotalAmount { get; set; }
        public decimal? DocTaxAmount { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? BaseTaxAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        public string AmtCurrency { get; set; }
    }
}

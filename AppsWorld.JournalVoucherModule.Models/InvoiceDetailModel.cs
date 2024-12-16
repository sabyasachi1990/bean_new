using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class InvoiceDetailModel
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
		public decimal? BalanceAmount { get; set; }
		public decimal? DoubtfulDebitTotalAmount { get; set; }
		public virtual List<InvoiceCreditNoteModel> InvoiceCreditNoteModels { get; set; }

		public virtual List<InvoiceDoubtFulDebitModel> InvoiceDoubtFulDebitModels { get; set; }
    }
}

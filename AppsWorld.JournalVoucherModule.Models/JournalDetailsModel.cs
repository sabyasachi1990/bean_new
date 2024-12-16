using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JournalDetailsModel
    {
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public bool? Disallowable { get; set; }
		public long TaxId { get; set; }
        public string TaxCode { get; set; }
        public string TaxType { get; set; }
        public double? TaxRate { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? BaseDebit { get; set; }
        public decimal? BaseCredit { get; set; }
        public decimal? DocTaxDebit { get; set; }
        public decimal? DocTaxCredit { get; set; }
        public decimal? BaseTaxDebit { get; set; }
        public decimal? BaseTaxCredit { get; set; }
		public virtual InvoiceDetailModel InvoiceDetailModel { get; set; }
        public virtual CreditNoteDetailModel CreditNoteDetailModel { get; set; }
        public virtual DebitNoteDetailModel DebitNoteDetailModel { get; set; }
        public virtual DoubtfulDebtDetailModel DoubtfulDebtDetailModel { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Models
{
    public class InvoiceDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public long COAId { get; set; }
        public Nullable<long> TaxId { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        public decimal DocAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? BaseTaxAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        public string TaxIdCode { get; set; }
        public string TaxType { get; set; }

    }
}

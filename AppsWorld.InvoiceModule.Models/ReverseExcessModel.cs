using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class ReverseExcessModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public decimal DocAmount { get; set; }
        //public string Remarks { get; set; }
        public long? TaxId { get; set; }
        public long? COAId { get; set; }
        public string RecordStatus { get; set; }
        public decimal? DocTaxAmount { get; set; }
        public decimal? DocTotalAmount { get; set; }
        public string TaxIdCode { get; set; }
        public double? TaxRate { get; set; }
        public int? RecOrder { get; set; }
        public string ItemDescription { get; set; }
    }
}

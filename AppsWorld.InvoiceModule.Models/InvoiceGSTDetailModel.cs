using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class InvoiceGSTDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public Nullable<long> TaxId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class InvoiceDocumentDetailModel
    {
        public Guid? Id { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocNo { get; set; }
        public decimal? Amount { get; set; }
        public string DocType { get; set; }
        public bool? IsHyperLinkEnable { get; set; }
    }
}

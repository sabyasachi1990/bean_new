using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class DDReverseExcessModel
    {
        public System.Guid Id { get; set; }
        public System.Guid DoubtfulDebtAllocationId { get; set; }
        public string DocCurrency { get; set; }
        public decimal AllocateAmount { get; set; }
        public string DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public Guid? EntityId { get; set; }
        public string RecordStatus { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}

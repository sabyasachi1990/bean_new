using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Models.Models
{
    public class SOAOutstandingAmount
    {
        public decimal? CreditNoteBalance { get; set; }
        public string Currency { get; set; }
        public string Remarks { get; set; }
        public string DocBalance { get; set; }
        public string DocDate { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string DocumentTotal { get; set; }
        public long ServiceCompanyId { get; set; }
    }
    public class OutstandingTotals
    {
        public string SubTotal { get; set; }
        public string Currency { get; set; }
    }
}

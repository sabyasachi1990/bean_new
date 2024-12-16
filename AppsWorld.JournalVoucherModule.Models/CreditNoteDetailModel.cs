using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class CreditNoteDetailModel
    {
        public Nullable<Guid> Item { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}

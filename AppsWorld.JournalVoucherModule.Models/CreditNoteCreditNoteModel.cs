using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class CreditNoteCreditNoteModel
    {
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string SystemRefNo { get; set; }
        public decimal Amount { get; set; }
    }
}

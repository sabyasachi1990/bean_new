using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Models
{
    public class MemoApplicationModel
    {
        public System.Guid MemoDetailId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string SystemRefNo { get; set; }
        public decimal Ammount { get; set; }
        public string DocType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models.V3
{
    public class CommonModel
    {
        public long CompanyId { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public bool SamePeriod { get; set; }
        public string Period { get; set; }
        public long Frequency { get; set; }
        public string CompanyName { get; set; }
       // public bool IsZeroAccount { get; set; } = false;
        public bool IsInterco { get; set; }
    }
}

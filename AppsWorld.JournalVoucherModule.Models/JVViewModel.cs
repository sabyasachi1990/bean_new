using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JVViewModel
    {
        public Guid Id { get; set; }
        public string SystemReferenceNo { get; set; }
        public string DocType { get; set; }
        public string Type { get; set; }
    }
}

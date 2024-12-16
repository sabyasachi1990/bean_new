using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class InBoundFilesModel
    {
        [DisplayName("event")]
        public string @event { get; set; }
        public Guid docId { get; set; }
        public string receivedAt { get; set; }
        public string  invoiceFileUrl { get; set; }
        public string evidenceFileUrl { get; set; }
        public string expiresAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class WorkFlowMongoFile
    {
        public string MongoId { get; set; }
        public long CompanyId { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public Guid InvoiceId { get; set; }
    }
}

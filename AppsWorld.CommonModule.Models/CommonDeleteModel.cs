using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class CommonDeleteModel
    {
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string DeletedBy { get; set; }
        public string TransactionIds { get; set; }
    }
}

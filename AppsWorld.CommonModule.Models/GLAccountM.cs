using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class GLAccountM
    {
        public string CompanyId { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public string ServiceEntityName { get; set; }
        public string COANames { get; set; }
        public int ExcludeCreatedItems { get; set; }
    }
}

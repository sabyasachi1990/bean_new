using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
   public class AutoNumberViewModel
    {
        public long CompanyId { get; set; }
        public string  Type { get; set; }
        public List<string> SystemRefNo { get; set; }
        public string  CompanyCode { get; set; }
        public string  ServiceGroupCode { get; set; }
    }
}

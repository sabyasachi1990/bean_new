using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class CommonLockModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public bool IsLocked { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string Version { get; set; }
    }
}

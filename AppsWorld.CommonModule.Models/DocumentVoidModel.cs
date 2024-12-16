using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class DocumentVoidModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string PeriodLockPassword { get; set; }
        public string DocType { get; set; }
        public string UserCreated { get; set; }
        public string DocNo { get; set; }
        public bool? IsDelete { get; set; }
        public string ModifiedBy { get; set; }
        public string Version { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

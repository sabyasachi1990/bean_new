using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class DocumentVoidModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string PeriodLockPassword { get; set; }
        public Nullable<Guid> PayrollId { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Version { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Models
{
    public class DocumentVoidModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string PeriodLockPassword { get; set; }

    }
}

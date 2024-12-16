using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Models
{
    public class RevaluationCancelModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public DateTime RevalDate { get; set; }
        public string DocState { get; set; }
        public string PeriodLockPassword { get; set; }
        public string Version { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Models
{
    public class RevaluationModelK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public DateTime? RevaluationDate { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string ServiceCompanyName { get; set; }
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string DocState { get; set; }
        public string DocNo { get; set; }
        public double? NetAmount { get; set; }
        public bool? IsLocked { get; set; }
        public string DocType { get; set; }
    }
}

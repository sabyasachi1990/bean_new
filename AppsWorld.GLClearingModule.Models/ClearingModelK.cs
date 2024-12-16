using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Models
{
    public class ClearingModelK
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string SystemRefNo { get; set; }
        //public decimal DocAmount { get; set; }
        //public decimal BaseAmount { get; set; }
        public string DocDate { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public string ServiceCompanyName { get; set; }
        public string DocumentState { get; set; }
        public string DocDescription { get; set; }
        public string AccountName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string UserCreated { get; set; }
        public bool? IsLocked { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class OpeningBalanceModelK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string ServiceCompanyName { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string  UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public bool Status { get; set; }
        public string SaveType { get; set; }
        public bool? IsLocked { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}

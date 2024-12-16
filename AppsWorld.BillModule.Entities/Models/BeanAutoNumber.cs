using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Entities
{
    public partial class BeanAutoNumber : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<long> ModuleMasterId { get; set; }
        public string EntityType { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public string GeneratedNumber { get; set; }
        public Nullable<int> CounterLength { get; set; }
        public Nullable<int> MaxLength { get; set; }
        public Nullable<long> StartNumber { get; set; }
        public string Reset { get; set; }
        public string Preview { get; set; }
        public Nullable<bool> IsResetbySubsidary { get; set; }
        public Nullable<int> Status { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Variables { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsDisable { get; set; }
    }
}

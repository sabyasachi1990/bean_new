using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace AppsWorld.CashSalesModule.Entities.Models
{
    public partial class AutoNumberCompany : Entity
    {
        public System.Guid Id { get; set; }
        public long SubsideryCompanyId { get; set; }
        public Nullable<System.Guid> AutonumberId { get; set; }
        public string GeneratedNumber { get; set; }
        public virtual AutoNumber AutoNumber { get; set; }
        //public virtual Company Company { get; set; }
    }
}
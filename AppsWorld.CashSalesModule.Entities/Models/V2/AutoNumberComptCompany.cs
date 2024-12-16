using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public partial class AutoNumberComptCompany : Entity
    {
        public System.Guid Id { get; set; }
        public long SubsideryCompanyId { get; set; }
        public Nullable<System.Guid> AutonumberId { get; set; }
        public string GeneratedNumber { get; set; }
    }
}

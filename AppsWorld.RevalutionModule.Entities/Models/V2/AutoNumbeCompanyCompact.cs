using System;
using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class AutoNumberCompanyCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long SubsideryCompanyId { get; set; }
        public Nullable<System.Guid> AutonumberId { get; set; }
        public string GeneratedNumber { get; set; }
    }
}

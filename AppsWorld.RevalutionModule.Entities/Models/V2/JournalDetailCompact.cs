using System;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class JournalDetailCompact : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid JournalId { get; set; }
        public long COAId { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string BaseCurrency { get; set; }
        public string DocCurrency { get; set; }
    }
}

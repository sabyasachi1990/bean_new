using System;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.MasterModule.Entities
{
    public partial class JournalDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid JournalId { get; set; }
        public long COAId { get; set; }
		public Guid? DocumentId { get; set; }
		public Guid? DocumentDetailId { get; set; }
        public string Currency { get; set; }
        public long? ServiceCompanyId { get; set; }
    }
}

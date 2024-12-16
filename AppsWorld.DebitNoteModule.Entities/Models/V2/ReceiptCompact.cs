using System;
using Repository.Pattern.Ef6;

namespace AppsWorld.DebitNoteModule.Entities.V2
{
    public partial class ReceiptCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocNo { get; set; }
        public DateTime? CreatedDate { get; set; }
         
    }
}

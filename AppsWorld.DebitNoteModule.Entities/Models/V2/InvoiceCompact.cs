using Repository.Pattern.Ef6;
using System;

namespace AppsWorld.DebitNoteModule.Entities.V2
{
    //[Table("Bean.Invoice")]
    public partial class InvoiceCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
    }
}

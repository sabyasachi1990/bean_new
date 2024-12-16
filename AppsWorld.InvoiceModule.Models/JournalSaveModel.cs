using System;
namespace AppsWorld.InvoiceModule.Models
{
    public class JournalSaveModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocNo { get; set; }
        public DateTime? ReversalDate { get; set; }
        public string ModifiedBy { get; set; }
        //public Guid? DocumentId { get; set; }
    }
}

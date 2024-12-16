using System;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.BillModule.Entities
{
    public partial class ReceiptDetailCompact : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid ReceiptId { get; set; }
        public string DocumentNo { get; set; }
        public String DocumentState { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string Currency { get; set; }
        public decimal ReceiptAmount { get; set; }
        public virtual ReceiptCompact Receipts { get; set; }
    }
}

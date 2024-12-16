using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace AppsWorld.InvoiceModule.Entities
{
    public partial class PaymentDetailCompact : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid PaymentId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentState { get; set; }
        public string Currency { get; set; }
        public decimal PaymentAmount { get; set; }
        public System.Guid DocumentId { get; set; }
        public virtual PaymentCompact Payment { get; set; }
    }
}

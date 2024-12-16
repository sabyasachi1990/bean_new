using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class PaymentDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid PaymentId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentState { get; set; }
        public string Nature { get; set; }
        public decimal DocumentAmmount { get; set; }
        public decimal AmmountDue { get; set; }
        public string Currency { get; set; }
        public decimal PaymentAmount { get; set; }
        public System.Guid DocumentId { get; set; }
        public long? ServiceCompanyId { get; set; }
        //public virtual Payment Payment { get; set; }
    }
}

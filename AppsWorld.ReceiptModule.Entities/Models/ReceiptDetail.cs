using System;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class ReceiptDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid ReceiptId { get; set; }

        public Guid DocumentId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentState { get; set; }
        public string Nature { get; set; }
        public decimal DocumentAmmount { get; set; }
        public decimal AmmountDue { get; set; }
        public string Currency { get; set; }
        public decimal ReceiptAmount { get; set; }
      //  [NotMapped]
        //public string RecordStatus { get; set; }
        public int? RecOrder { get; set; }
        public long? ServiceCompanyId { get; set; }
       // public string ClearingState { get; set; }

        public decimal? RoundingAmount { get; set; }
        //public virtual Receipt Receipt { get; set; }
    }
}

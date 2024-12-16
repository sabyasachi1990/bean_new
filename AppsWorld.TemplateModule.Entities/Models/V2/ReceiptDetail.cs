using System;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public partial class ReceiptDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid ReceiptId { get; set; }
        public string DocumentNo { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public String DocumentState { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string Currency { get; set; }
        public decimal DocumentAmmount { get; set; }
        public decimal AmmountDue { get; set; }
        public long ServiceCompanyId { get; set; }

        //public string BankReceiptAmmount { get; set; }

        //public string PaymentAmount { get; set; }

        public decimal ReceiptAmount { get; set; }


        public virtual Receipt Receipts { get; set; }
    }
}

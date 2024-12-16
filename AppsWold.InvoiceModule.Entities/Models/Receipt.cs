using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.InvoiceModule.Entities
{
    public partial class Receipt : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public string SystemRefNo { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }

        public decimal BankReceiptAmmount { get; set; }
        public string BankReceiptAmmountCurrency { get; set; }

        public string DocCurrency { get; set; }

        public Nullable<decimal> ReceiptApplicationAmmount { get; set; }

        public string ModeOfReceipt { get; set; }

        public decimal GrandTotal { get; set; }

        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public System.Guid EntityId { get; set; }
        public string DocumentState { get; set; }
        public long ServiceCompanyId { get; set; }


        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}

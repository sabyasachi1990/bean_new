using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.BillModule.Entities
{
    public partial class ReceiptCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ReceiptApplicationAmmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocumentState { get; set; }
        public long ServiceCompanyId { get; set; }
        [ForeignKey("ReceiptId")]
        public virtual ICollection<ReceiptDetailCompact> ReceiptDetails { get; set; }
    }
}

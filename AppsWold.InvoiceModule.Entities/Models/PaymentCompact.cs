using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Entities
{
    public partial class PaymentCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string PaymentApplicationCurrency { get; set; }
        public Nullable<decimal> PaymentApplicationAmmount { get; set; }
        public string DocumentState { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocCurrency { get; set; }
        public long ServiceCompanyId { get; set; }
        public virtual ICollection<PaymentDetailCompact> PaymentDetails { get; set; }
    }
}

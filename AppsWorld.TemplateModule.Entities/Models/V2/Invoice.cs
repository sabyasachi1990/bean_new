
using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    //[Table("Bean.Invoice")]
    public partial class Invoice : Entity
    {
      
        public System.Guid Id { get; set; }
        public string DocNo { get; set; }
        public long? CreditTermsId { get; set; }
        public System.DateTime DocDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string PONo { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public bool? IsOBInvoice { get; set; }
        public string DocumentState { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocCurrency { get; set; }
        public decimal BalanceAmount { get; set; }
        [DataType("decimal(15 ,10")]
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string DocDescription { get; set; }
        public long ServiceCompanyId { get; set; }
        public Guid EntityId { get; set; }
        public bool IsGstSettings { get; set; }
        [ForeignKey("EntityId")]
        public virtual BeanEntity BeanEntity { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }

    }
}

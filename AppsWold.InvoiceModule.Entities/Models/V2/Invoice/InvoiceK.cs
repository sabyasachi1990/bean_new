using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    //[Table("Bean.Invoice")]
    public partial class InvoiceK : Entity
    {
         
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocSubType { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string InternalState { get; set; }
        public string PONo { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public System.Guid EntityId { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        public string DocumentState { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string InvoiceNumber { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string DocDescription { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExtensionType { get; set; }

        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        public bool? IsWorkFlowInvoice { get; set; }
        public bool? IsOBInvoice { get; set; }
        public decimal BaseAmount { get; set; }
        public Nullable<Guid> RecurInvId { get; set; }
        public Nullable<int> RepEveryPeriodNo { get; set; }
        public Nullable<System.DateTime> RepEndDate { get; set; }
        public DateTime? LastPostedDate { get; set; }
        public DateTime? NextDue { get; set; }
        public int? Counter { get; set; }
    }
}

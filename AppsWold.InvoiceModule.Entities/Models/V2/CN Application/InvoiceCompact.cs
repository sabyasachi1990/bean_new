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
    public partial class InvoiceCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public System.DateTime DocDate { get; set; }
        public System.Guid EntityId { get; set; }
        public string DocNo { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string Nature { get; set; }
        public string DocumentState { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string InternalState { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal? AllocatedAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocCurrency { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string DocDescription { get; set; }
        public Guid? OpeningBalanceId { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool? IsWorkFlowInvoice { get; set; }
        public string DocSubType { get; set; }
        public bool? IsOBInvoice { get; set; }
        public Guid? DocumentId { get; set; }
        public long? CreditTermsId { get; set; }
        public string ExCurrency { get; set; }
        public string GSTExCurrency { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public string UserCreated { get; set; }
        public string Remarks { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string PONo { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
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
        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? RoundingAmount { get; set; }
    }
}

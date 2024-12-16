using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Entities
{
    public class InvoiceCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public System.Guid EntityId { get; set; }
        public long? CreditTermsId { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        [DataType("decimal(15 ,10")]
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public string DocumentState { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? AllocatedAmount { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public long? ServiceCompanyId { get; set; }
        public bool? IsGstSettings { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public bool? NoSupportingDocs { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
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
        public Guid? DocumentId { get; set; }
        public string InternalState { get; set; }
        public bool? IsOBInvoice { get; set; }
        public Guid? OpeningBalanceId { get; set; }
        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? RoundingAmount { get; set; }
    }
}

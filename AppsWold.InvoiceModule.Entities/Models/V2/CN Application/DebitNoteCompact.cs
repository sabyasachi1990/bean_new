using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public partial class DebitNoteCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Nature { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string DocumentState { get; set; }
        public decimal? AllocatedAmount { get; set; }
        public System.Guid EntityId { get; set; }
        public string DocCurrency { get; set; }
        public decimal GrandTotal { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCreated { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
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
        public long? ServiceCompanyId { get; set; }
        public string DocSubType { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Remarks { get; set; }
        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? RoundingAmount { get; set; }

    }
}

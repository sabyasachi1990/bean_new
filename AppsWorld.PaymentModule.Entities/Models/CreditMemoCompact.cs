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
    public class CreditMemoCompact:Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime DocDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string DocNo { get; set; }
        public DateTime PostingDate { get; set; }
        public System.Guid EntityId { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public string DocumentState { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string UserCreated { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
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
        public string CreditMemoNumber { get; set; }
        public Nullable<long> ServiceCompanyId { get; set; }
        public string DocType { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<decimal> AllocatedAmount { get; set; }
        public Guid? OpeningBalanceId { get; set; }
        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? RoundingAmount { get; set; }
    }
}

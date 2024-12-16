using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public partial class CashSaleK :Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string PONo { get; set; }
        public System.Guid? EntityId { get; set; }
        public string ModeOfReceipt { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public string ReceiptrefNo { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string DocumentState { get; set; }
        public decimal GrandTotal { get; set; }
        public string UserCreated { get; set; }
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
       
        public Nullable<long> ServiceCompanyId { get; set; }
        public string DocType { get; set; }
        public long COAId { get; set; }
        
    }
}

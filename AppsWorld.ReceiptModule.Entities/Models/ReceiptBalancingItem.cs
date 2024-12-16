using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class ReceiptBalancingItem : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid ReceiptId { get; set; }
        public string ReciveORPay { get; set; }
        public long COAId { get; set; }
        public string Account { get; set; }
        public Nullable<bool> IsDisAllow { get; set; }
        public long? TaxId { get; set; }
        //public string TaxCode { get; set; }
        public double? TaxRate { get; set; }
        public string TaxType { get; set; }
        public string Currency { get; set; }
        public decimal DocAmount { get; set; }
        public decimal? DocTaxAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool? IsPLAccount { get; set; }
        [NotMapped]
        public string RecordStatus { get; set; }
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

        public int? RecOrder { get; set; }
        
        //[NotMapped]
        //public string ServiceCompanyName { get; set; }
       // public string ClearingState { get; set; }
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual Receipt Receipt { get; set; }
        public string TaxIdCode { get; set; }

        //[ForeignKey("TaxId")]
        //public virtual TaxCode TaxCode1 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class RevalutionDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid RevalutionId { get; set; }
        public Nullable<long> COAId { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string DocumentType { get; set; }
        public string DocumentSubType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentDescription { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public Nullable<System.Guid> DocId { get; set; }
        public Nullable<decimal> ExchangerateOld { get; set; }
        public Nullable<decimal> ExchangerateNew { get; set; }
        public string BaseCurrency { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> DocCurrencyAmount { get; set; }
        public Nullable<decimal> BaseCurrencyAmount1 { get; set; }
        public Nullable<decimal> BaseCurrencyAmount2 { get; set; }
        public decimal? UnrealisedExchangegainorlose { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public long? ServiceEntityId { get; set; }
        public bool? IsChecked { get; set; }
        public decimal? DocBal { get; set; }

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

        public Nullable<System.DateTime> PostingDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        //  public virtual Revaluation Revalution { get; set; }
    }
}

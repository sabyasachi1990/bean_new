using AppsWorld.BankTransferModule.Entities.Models;
using AppsWorld.CommonModule.Entities;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.BankTransferModule.Entities
{
    public partial class BankTransfer : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string SystemRefNo { get; set; }
        public string DocDescription { get; set; }
        public System.DateTime TransferDate { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public string DocNo { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public bool IsGstSetting { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsMultiCompany { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public string ModeOfTransfer { get; set; }
        public string TransferRefNo { get; set; }
        public Nullable<decimal> SystemCalculatedExchangeRate { get; set; }
        public Nullable<decimal> VarianceExchangeRate { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public string DocumentState { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int? ClearCount { get; set; }
        Framework.RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public Framework.RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (Framework.RecordStatusEnum)value; }
        }
        [Timestamp]
        public byte[] Version { get; set; }
        public bool? IsInterCompany { get; set; }
        public bool? IsIntercoClearing { get; set; }
        public bool? IsIntercoBilling { get; set; }
        public bool? IsLocked { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<BankTransferDetail> BankTransferDetails { get; set; }
        public virtual List<SettlementDetail> SettlementDetails { get; set; }
    }
}

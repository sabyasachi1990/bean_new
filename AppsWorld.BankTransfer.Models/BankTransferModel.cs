using AppsWorld.BankTransferModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Models;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Models
{
    public class BankTransferModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string SystemRefNo { get; set; }
        public string DocDescription { get; set; }
        [Required(ErrorMessage = "Transfer Date field is Required")]
        public System.DateTime TransferDate { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        [Required(ErrorMessage = "DocNo field is Required")]
        public string DocNo { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public bool IsGstSetting { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsMultiCompany { get; set; }

        public Nullable<bool> NoSupportingDocument { get; set; }
        public string ModeOfTransfer { get; set; }
        public string TransferRefNo { get; set; }
        [DataType("decimal(15,10")]
        public Nullable<decimal> SystemCalculatedExchangeRate { get; set; }
        [DataType("decimal(15,10")]
        public string VarianceExchangeRate { get; set; }
        // [Required(ErrorMessage = "ExchangeRate field is Required")]
        [DataType("decimal(15,10")]
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public string DocumentState { get; set; }
        public string Remarks { get; set; }
        public bool? IsLocked { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Version { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Required(ErrorMessage = "Status Required")]
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
        public bool IsBankReconciliation { get; set; }
        public Guid JournalId { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public bool IsInterCompany { get; set; }
        public bool? IsDocNoEditable { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public bool? IsIntercoClearing { get; set; }
        public bool? IsIntercoBilling { get; set; }
        public bool? IsClearing { get; set; }
        public bool? IsIB { get; set; }

        public List<TailsModel> TileAttachments { get; set; }
        public string Path { get; set; }

        //public virtual List<JVViewModel> JVViewModels { get; set; }
        public virtual ICollection<BankTransferDetailModel> BankTransferDetailsModel { get; set; }
        public virtual ICollection<SettlementDetailModel> SettlementDetailModels { get; set; }
        public bool IsModify { get; set; }
    }
}

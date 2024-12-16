
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Models
{
    public class CreditMemoApplicationModel
    {
        public CreditMemoApplicationModel()
        {
            this.CreditMemoApplicationDetailModels = new List<CreditMemoApplicationDetailModel>();
            this.ReverseExcessModels = new List<ReverseExcessModel>();
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Guid CreditMemoId { get; set; }
        public string DocNo { get; set; }
        public System.DateTime DocDate { get; set; }
        public string CreditMemoApplicationNumber { get; set; }
        public string DocCurrency { get; set; }
        public string ModifiedBy { get; set; }
        public string DocState { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public System.DateTime CreditMemoApplicationDate { get; set; }
        public Nullable<System.DateTime> CreditMemoApplicationResetDate { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal CreditMemoAmount { get; set; }
        public Guid? DocumentId { get; set; }
        public decimal CreditMemoBalanceAmount { get; set; }
        public string Remarks { get; set; }
        public bool? IsModify { get; set; }
        public string Version { get; set; }
        public bool? IsLocked { get; set; }
        CreditMemoApplicationStatus _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public CreditMemoApplicationStatus Status
        {
            get
            {
                return _status;
            }
            set { _status = (CreditMemoApplicationStatus)value; }
        }
        public Guid JournalId { get; set; }
        public string DocSubType { get; set; }
        public string PeriodLockPassword { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string ClearingState { get; set; }
        public Guid? EntityId { get; set; }
        public string EntityName { get; set; }
        public bool? IsRevExcess { get; set; }
        public bool? IsOffset { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public virtual List<CreditMemoApplicationDetailModel> CreditMemoApplicationDetailModels { get; set; }
        public virtual List<ReverseExcessModel> ReverseExcessModels { get; set; }
    }
}

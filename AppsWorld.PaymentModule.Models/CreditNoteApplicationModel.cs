using AppsWorld.CommonModule.Infra;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Models
{
    public class CreditNoteApplicationModel
    {
        public CreditNoteApplicationModel()
        {
            this.CreditNoteApplicationDetailModels = new List<CreditNoteApplicationDetailModel>();
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Guid InvoiceId { get; set; }
        public string DocNo { get; set; }
        public System.DateTime DocDate { get; set; }
        public string CreditNoteApplicationNumber { get; set; }
        public string DocCurrency { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public System.DateTime CreditNoteApplicationDate { get; set; }
        public Nullable<System.DateTime> CreditNoteApplicationResetDate { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal CreditNoteAmount { get; set; }
        public decimal CreditNoteBalanceAmount { get; set; }
        public bool? IsRevExcess { get; set; }
        public bool? IsGstSettings { get; set; }
        public string Remarks { get; set; }
        public bool? IsICActive { get; set; }
        public Guid? DocumentId { get; set; }
        CreditNoteApplicationStatus _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public CreditNoteApplicationStatus Status
        {
            get
            {
                return _status;
            }
            set { _status = (CreditNoteApplicationStatus)value; }
        }
        public Guid JournalId { get; set; }
        public string DocSubType { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public bool? IsOffset { get; set; }
        public string PeriodLockPassword { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string EntityName { get; set; }
        public string Version { get; set; }
        public virtual List<CreditNoteApplicationDetailModel> CreditNoteApplicationDetailModels { get; set; }
    }
}

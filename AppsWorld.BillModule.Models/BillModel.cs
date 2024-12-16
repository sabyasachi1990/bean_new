using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Models;

namespace AppsWorld.BillModule.Models
{
    public class BillModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Nature { get; set; }
        public string DocNo { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DueDate { get; set; }
        public string DocSubType { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string BaseCurrency { get; set; }
        public bool ISMultiCurrency { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsNoSupportingDocument { get; set; }
        public Guid JournalId { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public string EntityName { get; set; }
        public decimal? GstExchangeRate { get; set; }
        public string GstReportingCurrency { get; set; }
        public decimal GrandTotal { get; set; }
        public string UserCreated { get; set; }
        public long ServiceCompanyId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Version { get; set; }
        public bool? IsGSTCurrencyRateChanged { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public Guid EntityId { get; set; }
        public long? CreditTermId { get; set; }
        public string Path { get; set; }
        public string DocumentState { get; set; }
        public string SystemReferenceNumber { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public string DocCurrency { get; set; }
        public bool? IsGSTApplied { get; set; }
        public string DocDescription { get; set; }
        public Nullable<Guid> SyncPayrollId { get; set; }
        public Nullable<Guid> PayrollId { get; set; }
        public string DocType { get; set; }
        public bool? IsExternal { get; set; }
        public bool? IsModify { get; set; }
        public bool? IsLocked { get; set; }
        public string PeriodLockPassword { get; set; }
        public bool IsFromPayroll { get; set; }
        public bool? IsDocNoEditable { get; set; }
        public Guid? OpeningBalanceId { get; set; }
        public System.Guid? PeppolDocumentId { get; set; } //Peppol Related
        public bool IsFromPeppol { get; set; }
        RecordStatusEnum _status;

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [FrameWork.StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = value; }
        }
        public List<BillDetailModel> BillDetailModels { get; set; }
        public List<TaxModel> TaxModels { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public CommonModule.Infra.LookUpCategory<string> CurrencyLU { get; set; }
        public List<CommonModule.Infra.LookUp<string>> TermsOfPaymentLU { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public List<DocRepositoryModel> Attachments { get; set; }
        public List<TailsModel> TileAttachments { get; set; }
    }
}

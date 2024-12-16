using System;
using System.Collections.Generic;
using AppsWorld.JournalVoucherModule.Entities;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using FrameWork;
using AppsWorld.Framework;
using Newtonsoft.Json.Converters;
using AppsWorld.JournalVoucherModule.Model;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JournalModel
    {
        //public ServiceCompanyModel ServiceCompanyMOdels { get; set; }
        //public SegmentCategoryModel SegmentCategory { get; set; }
        //public CurrencyModel Currency { get; set; }
        //public JournalModel()
        //{
        //    this.ServiceCompanyMOdels = new ServiceCompanyModel();
        //    this.SegmentCategory = new SegmentCategoryModel();
        //    this.Currency = new CurrencyModel();
        //    this.JournalDetailModels = new List<JournalDetailModel>();
        //}
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Nature { get; set; }
        public string DocNo { get; set; }
        public string DocDescription { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DueDate { get; set; }
        [DataType("decimal(15,10")]
        public decimal? ExchangeRate { get; set; }
        public string BaseCurrency { get; set; }
        public string SystemORManual { get; set; }
        public long? ServiceCompanyId { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> exDurationTo { get; set; }
        public Nullable<System.DateTime> GstdurationFrom { get; set; }
        public Nullable<System.DateTime> GstDurationTo { get; set; }
        public bool? ISMultiCurrency { get; set; }
        public bool? IsGstSettings { get; set; }
        public string Version { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        //public bool ISSegmentReporting { get; set; }
        //public bool ISGstDeRegistered { get; set; }
        public string Remarks { get; set; }
        [DataType("decimal(15,10")]
        public decimal? GstExchangeRate { get; set; }
        public string GstReportingCurrency { get; set; }
        //public bool? ISAllowDisAllow { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public bool? IsGSTCurrencyRateChanged { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        //public long? SegmentMasterid1 { get; set; }
        //public long? SegmentMasterid2 { get; set; }
        //public long? SegmentDetailid1 { get; set; }
        //public long? SegmentDetailid2 { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        //public bool? IsSegmentActive1 { get; set; }
        //public bool? IsSegmentActive2 { get; set; }
        public string DocumentState { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string ExCurrency { get; set; }
        public string RecurringJournalName { get; set; }
        public Nullable<DateTime> FrequencyEndDate { get; set; }
        public Nullable<int> FrequencyValue { get; set; }
        public string FrequencyType { get; set; }
        public Nullable<DateTime> ReversalDate { get; set; }
        public decimal GSTTotalAmount { get; set; }
        public bool IsPosted { get; set; }
        public string DocCurrency { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? BaseAmount { get; set; }
        public DateTime? BankClearingDate { get; set; }
        public string VendorType { get; set; }
        public string EntityName { get; set; }
        public string CustCreditTerms { get; set; }
        public string VenCreditTerms { get; set; }
        public string PONo { get; set; }
        public bool? Repeating { get; set; }
        public string ReceiptrefNo { get; set; }
        public string PaymentRefNo { get; set; }
        public string DepositRefNo { get; set; }
        public string WithdrawalRefNo { get; set; }
        public string ActualSysRefNo { get; set; }
        public bool? IsCopyReversal { get; set; }
        public bool? IsLocked { get; set; }
        //public string Mode { get; set; }
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

        public string PeriodLockPassword { get; set; }
        public bool? IsAutoReversalJournal { get; set; }
        public bool? IsRecurringJournal { get; set; }
        public bool? IsAllowableNonAllowable { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string ReverseParentRef { get; set; }
        public string ReverseChildRef { get; set; }
        public Guid? ReverseParentRefId { get; set; }
        public Guid? ReverseChildRefId { get; set; }
        public bool? IsFor { get; set; }
        public string CreationType { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public Nullable<double> GrandDocDebitTotal { get; set; }
        public Nullable<double> GrandDocCreditTotal { get; set; }
        public Nullable<double> GrandBaseDebitTotal { get; set; }
        public Nullable<double> GrandBaseCreditTotal { get; set; }
        public string TransferRefNo { get; set; }
        public bool? IsWithdrawal { get; set; }
        public DateTime? NextDue { get; set; }
        public DateTime? LastPosted { get; set; }
        public Guid? RecurringJournalId { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCopy { get; set; }
        public int? Counter { get; set; }
        public bool? IsPostChecked { get; set; }
        public Guid? ParentId { get; set; }
        public bool? IsDocNoEditable { get; set; }
        public Guid? DocumentId { get; set; }
        public bool? IsDeleted { get; set; }
        public string Path { get; set; }
        public bool? reverseLocked { get; set; }
        public string InternalState { get; set; }
        public bool? IsBaseCurrencyJV { get; set; }
        public virtual ICollection<JournalDetailModel> JournalDetailModels { get; set; }
        //public virtual ICollection<JournalGSTDetail> JournalGSTDetails { get; set; }
        public List<TailsModel> TileAttachments { get; set; }
    }
    public class TailsModel
    {
        public string Name { get; set; }       //Name of file or folder
        public string FileSize { get; set; }  //size of file
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsSystem { get; set; }
        public bool IsFolder { get; set; }      //is folder or file
        public string Ext { get; set; }  //extension of file (folder does not get the extension)
        public string Description { get; set; }
        public string FileType { get; set; }   //Attached Files,Attachments,Tails
        public string Url { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
    }
    public class Tails
    {
        public long CompanyId { get; set; }
        public long FileShareName { get; set; }
        public string CursorName { get; set; }
        public string Path { get; set; }
        public List<TailsModel> LstTailsModel { get; set; }
    }
}

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
    public partial class JournalModelK
    {
        //public ServiceCompanyModel ServiceCompanyMOdels { get; set; }

        //public SegmentCategoryModel SegmentCategory { get; set; }
        //public CurrencyModel Currency { get; set; }

        public JournalModelK()
        {


            //this.ServiceCompanyMOdels = new ServiceCompanyModel();
            //this.SegmentCategory = new SegmentCategoryModel();
            //this.Currency = new CurrencyModel();
            //this.JournalDetailModelsK = new List<JournalDetailModelK>();

        }


        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        //public string CompanyName { get; set; }
        public string DocNo { get; set; }
        public string DocDescription { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime DocDate { get; set; }
        //public DateTime DueDate { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public bool? IsLocked { get; set; }
        //public string BaseCurrency { get; set; }
        //[DataType("decimal(15,10")]
        public string ExchangeRate { get; set; }

        public Nullable<double> GrandDocDebitTotal { get; set; }
        public Nullable<double> GrandDocCreditTotal { get; set; }
        public Nullable<double> GrandBaseDebitTotal { get; set; }
        public Nullable<double> GrandBaseCreditTotal { get; set; }
        public string SystemORManual { get; set; }

        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public string ReceiptrefNo { get; set; }
        //public string PaymentRefNo { get; set; }
        //public string DepositRefNo { get; set; }
        //public string WithdrawalRefNo { get; set; }
        //public string TransferRefNo { get; set; }
        //public string CPRefNo { get; set; }
        public string DocumentState { get; set; }
        public string SystemReferenceNumber { get; set; }
        //public string Mode { get; set; }
        //public bool IsMultiCurrency { get; set; }
        public string DocCurrency { get; set; }
        public bool? IsModify { get; set; }
        //public string EntityName { get; set; }
        //public string CustCreditTerms { get; set; }
        //public string VenCreditTerms { get; set; }
        //public string PONo { get; set; }
        //public bool? Repeating { get; set; }
        //public string VendorType { get; set; }
        public decimal? BalanceAmount { get; set; }
        public double? BaseAmount { get; set; }
        public Guid? DocumentId { get; set; }
        public bool? IsSystem { get; set; }
        //public DateTime? BankClearingDate { get; set; }
        //public string CreationType { get; set; }
        //public Nullable<bool> NoSupportingDocument { get; set; }
        //public decimal? BaseBalance { get; set; }

        public long? ServiceCompanyId { get; set; }
        public string ServiceCompanyName { get; set; }
        public string InternalState { get; set; }
        public DateTime? ReversalDate { get; set; }
        //public string Version { get; set; }
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

        public byte[] RowVersion
        {
            get;
            set;
        }

        public string Version
        {
            get
            {
                if (this.RowVersion != null)
                {
                    //return Convert.ToBase64String(this.Version);
                    return "0x" + string.Concat(Array.ConvertAll(RowVersion, x => x.ToString("X2")));
                }

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.RowVersion = null;
                }
                else
                {
                    this.RowVersion = Convert.FromBase64String(value);
                }
            }
        }

    }
}

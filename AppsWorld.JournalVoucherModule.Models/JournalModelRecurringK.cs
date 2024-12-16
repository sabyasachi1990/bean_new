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
    public partial class JournalModelRecurringK
    {
        //public ServiceCompanyModel ServiceCompanyMOdels { get; set; }

        ////public SegmentCategoryModel SegmentCategory { get; set; }
        ////public CurrencyModel Currency { get; set; }

        //public JournalModelRecurringK()
        //{

        //    this.ServiceCompanyMOdels = new ServiceCompanyModel();
        //    //this.SegmentCategory = new SegmentCategoryModel();
        //    //this.Currency = new CurrencyModel();
        //    //this.JournalDetailModelsK = new List<JournalDetailModelK>();

        //}


        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string ServiceCompanyName { get; set; }
        public string DocDescription { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string RecurringJournalName { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> FrequencyValue { get; set; }
        public string FrequencyType { get; set; }
        public Nullable<System.DateTime> FrequencyEndDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string DocCurrency { get; set; }
        public DateTime? LastPosted { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextDue { get; set; }
        //public string SystemRefNo { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string EntityName { get; set; }
        public string CustCreditTerms { get; set; }
        public string VenCreditTerms { get; set; }
        public string PONo { get; set; }
        public bool? Repeating { get; set; }
        public string VendorType { get; set; }
        public decimal? DocTotal { get; set; }
        public decimal? GrandDocDebitTotal { get; set; }
        public string DocumentState { get; set; }
        public string DocNo { get; set; }
        public string InternalState { get; set; }
        public DateTime? DocDate { get; set; }
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
        public bool? IsLocked { get; set; }
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

    }
}

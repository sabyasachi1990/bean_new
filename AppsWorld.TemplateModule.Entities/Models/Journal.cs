using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AppsWorld.Framework;
using FrameWork;

namespace AppsWorld.TemplateModule.Entities.Models
{
    public partial class Journal:Entity
    {
        public Journal()
        {
            this.JournalDetails = new List<JournalDetail>();
        }
        public System.Guid Id { get; set; }

        public System.Guid EntityId { get; set; }
        public long CompanyId { get; set; }
        public System.Guid? DocumentId { get; set; }

        public string DocNo { get; set; }
        public string DocCurrency { get; set; }

        //public long? ServiceCompanyId { get; set; }
        public string SystemReferenceNo { get; set; }

        public string DocType { get; set; }
        public Nullable<decimal> GrandDocDebitTotal { get; set; }
        public Nullable<decimal> GrandDocCreditTotal { get; set; }
        public string DocSubType { get; set; }
        public bool? IsWithdrawal { get; set; }
        public decimal? BalanceAmount { get; set; }
        public bool? IsGstSettings { get; set; }
        public long? ServiceCompanyId { get; set; }


        //CreditNoteApplicationStatus _status;
        //[Required]
        //[JsonConverter(typeof(StringEnumConverter))]
        //[StatusValue]
        //public CreditNoteApplicationStatus Status
        //{
        //    get
        //    {
        //        return _status;
        //    }
        //    set { _status = (CreditNoteApplicationStatus)value; }
        //}

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

        public string DocumentState { get; set; }
        public virtual List<JournalDetail> JournalDetails { get; set; }
    }
}

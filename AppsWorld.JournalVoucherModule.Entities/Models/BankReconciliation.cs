using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class BankReconciliation : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public long COAId { get; set; }
        public long ServiceCompanyId { get; set; }
        public System.DateTime BankReconciliationDate { get; set; }
        public bool? IsReRunBR { get; set; }
        public string State { get; set; }
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
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual List<BankReconciliationDetail> BankReconciliationDetails { get; set; }

    }
}

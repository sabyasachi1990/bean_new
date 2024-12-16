using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.Entities
{
    public partial class FinancialSetting : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string FinancialYearEnd { get; set; }
        public string TimeZone { get; set; }
        public Nullable<System.DateTime> PeriodLockDate { get; set; }
        public string PeriodLockDatePassword { get; set; }
        public Nullable<System.DateTime> EndOfYearLockDate { get; set; }

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
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string BaseCurrency { get; set; }
        public string LongDateFormat { get; set; }
        public string ShortDateFormat { get; set; }
        public string TimeFormat { get; set; }
        public Nullable<System.DateTime> PeriodEndDate { get; set; }
        public Nullable<bool> IsbaseCurrency { get; set; }
        public Nullable<bool> IsPosted { get; set; }
        //public virtual Company Company { get; set; }
    }
}

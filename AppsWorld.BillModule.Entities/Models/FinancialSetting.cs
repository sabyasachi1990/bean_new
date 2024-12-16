using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using AppsWorld.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.BillModule.Entities
{
    public partial class FinancialSetting : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }

        [Required]
        [StringLength(25)]
        public string FinancialYearEnd { get; set; }

        [Required]
        [StringLength(100)]
        public string TimeZone { get; set; }
        public Nullable<System.DateTime> PeriodLockDate { get; set; }
        [StringLength(50)]
        public string PeriodLockDatePassword { get; set; }
        public Nullable<System.DateTime> EndOfYearLockDate { get; set; }
      
        [StringLength(254)]
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [StringLength(254)]
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public string BaseCurrency { get; set; }

       // public virtual Company Company { get; set; }

        [StringLength(50)]
        public string BaseCurrency { get; set; }
        [StringLength(50)]
        public string LongDateFormat { get; set; }
        [StringLength(50)]
        public string ShortDateFormat { get; set; }
        [StringLength(50)]
        public string TimeFormat { get; set; }

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

        public bool? IsbaseCurrency { get; set; }
        public bool? IsPosted { get; set; }
        public Nullable<System.DateTime> PeriodEndDate { get; set; }
        [NotMapped]
        public string RecStatus { get; set; }
    }
}

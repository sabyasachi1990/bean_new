using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
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
        // public virtual Company Company { get; set; }
        [NotMapped]
        public string Date { get; set; }
        [NotMapped]
        public string Month { get; set; }
        [NotMapped]
        public bool IsEdit { get; set; }
        [NotMapped]
        public long? ModuleDetailId { get; set; }
    }
}

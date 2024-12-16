using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AppsWorld.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using FrameWork;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class FinancialSetting : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }

        
        public Nullable<System.DateTime> PeriodLockDate { get; set; }
        [StringLength(50)]
        public string PeriodLockDatePassword { get; set; }
        public Nullable<System.DateTime> EndOfYearLockDate { get; set; }
      
       

        [StringLength(50)]
        public string BaseCurrency { get; set; }
        

        //RecordStatusEnum _status;
        //[Required]
        //[JsonConverter(typeof(StringEnumConverter))]
        //[StatusValue]
        //public RecordStatusEnum Status
        //{
        //    get
        //    {
        //        return _status;
        //    }
        //    set { _status = (RecordStatusEnum)value; }
        //}

      
        public Nullable<System.DateTime> PeriodEndDate { get; set; }
        //[NotMapped]
        //public string RecStatus { get; set; }
    }
}

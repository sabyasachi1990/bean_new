using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class FinancialSettingCompact : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string FinancialYearEnd { get; set; }
        public Nullable<System.DateTime> PeriodLockDate { get; set; }
        public string PeriodLockDatePassword { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<System.DateTime> EndOfYearLockDate { get; set; }
        public Nullable<System.DateTime> PeriodEndDate { get; set; }
         
        //public virtual Company Company { get; set; }
    }
}

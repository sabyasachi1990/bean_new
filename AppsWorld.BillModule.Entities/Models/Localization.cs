using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.BillModule.Entities
{
    public partial class Localization:Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string LongDateFormat { get; set; }
        public string ShortDateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string BaseCurrency { get; set; }
        public string BusinessYearEnd { get; set; }
        public Nullable<int> Status { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string TimeZone { get; set; }
        public Nullable<decimal> DefaultWorkingHours { get; set; }
    }
}
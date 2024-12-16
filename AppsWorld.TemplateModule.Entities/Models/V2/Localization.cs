using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Entities.Models.V2
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
    }
}

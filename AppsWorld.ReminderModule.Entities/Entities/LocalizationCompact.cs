using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public partial class LocalizationCompact : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string ShortDateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string BaseCurrency { get; set; }
    }
}

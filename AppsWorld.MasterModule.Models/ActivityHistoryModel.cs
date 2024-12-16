using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class ActivityHistoryModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public int Type { get; set; }
        public string Activity { get; set; }
        public string Action { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid DocumentId { get; set; }
    }
}

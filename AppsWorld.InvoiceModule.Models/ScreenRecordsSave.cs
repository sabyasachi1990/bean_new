using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class ScreenRecordsSave
    {
        public string FeatureId { get; set; }
        public string RecordId { get; set; }
        public string recordName { get; set; }
        public bool? isAdd { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string CursorName { get; set; }
        public string ScreenName { get; set; }
        public string ReferenceId { get; set; }
        public long CompanyId { get; set; }
    }
}

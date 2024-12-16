using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class IncomeStatementModelsK
    {
        public Guid MainId { get; set; }
        public List<string> ColumnLists { get; set; }
        public List<LeadSheetTotalModelK> LeadSheetTotalModelK { get; set; }
        public string Name { get; set; } = "Total";
      
    }
}

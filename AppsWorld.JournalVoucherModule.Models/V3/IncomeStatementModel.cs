using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models.V3
{
    public class IncomeStatementModel
    {
        public Guid MainId { get; set; }
        public List<ColumnLsts> ColumnLists { get; set; }
        public List<LeadSheetTotalModel> LeadSheetTotalModels { get; set; }
        public List<YearModels> YearModels { get; set; }
        public bool IsFirstEngagement { get; set; }
        public string Name { get; set; } = "Total";
        public bool IsShowZero { get; set; }
        public bool IsVisible { get; set; }
    }
}

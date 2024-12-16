using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class LeadSheetTotalModelK
    {
        public Guid Id { get; set; }
        public Guid MainId { get; set; }
        public long? CompanyId { get; set; }

        public string Class { get; set; }
        public string LeadSheetName { get; set; }
        public Guid? LeadSheetId { get; set; }
        public string AccountClass { get; set; }

        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public string AccountName { get; set; }
        public string SubtotalName { get; set; }
        public string Code { get; set; }
        public Nullable<int> FRRecOrder { get; set; }


        public List<YearModels> YearModels { get; set; }
        public long? Recorder { get; set; }
        public long? ExcelExportRecorder { get; set; }
    }


    public class FinalBalanceSheetModelK
    {
        public List<LeadSheetTotalModelK> LeadSheetTotalModelK { get; set; }
        public List<string> ColumnLists { get; set; }
    }
}

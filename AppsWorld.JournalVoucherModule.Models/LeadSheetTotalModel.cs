using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class LeadSheetTotalModel
    {
        public LeadSheetTotalModel()
        {
            this.LeadSheetTotalModels = new List<LeadSheetTotalModel>();
            this.CategoryTotalModels = new List<CategoryTotalModel>();
            this.AccountModels = new List<AccountModel>();
        }
        public Guid Id { get; set; }
        public bool? IsLeadsheet { get; set; }
        public long? CompanyId { get; set; }
        public bool? IsGrandTotal { get; set; }
        public Guid MainId { get; set; }
        public string Name { get; set; }
        public Guid? CommonId { get; set; }
        public bool? IsCollapse { get; set; }
        public List<YearModels> YearModels { get; set; }
        public string RecordStatus { get; set; }
        public string Type { get; set; }
        public Guid? LeadSheetId { get; set; }
        public string Index { get; set; }
        public int? DupRecorder { get; set; }
        public string LeadSheetType { get; set; }
        public List<LeadSheetTotalModel> LeadSheetTotalModels { get; set; }
        public List<CategoryTotalModel> CategoryTotalModels { get; set; }
        public List<AccountModel> AccountModels { get; set; }
        public List<YearModels> OtherYearModels { get; set; }
        public string AccountClass { get; set; }
        public Guid? ParentId { get; set; }
        public int? Recorder { get; set; }
        public string SubTotals { get; set; }
        public string ScreenName { get; set; }
        public string ColorCode { get; set; }
        public bool? IsInfinity { get; set; }
        public bool IsShowZero { get; set; }
        public bool IsVisible { get; set; }
    }

    public class FinalBalanceSheetModel
    {
        public List<LeadSheetTotalModel> LeadSheetTotalModels { get; set; }
        //public List<string> ColumnList { get; set; }
        public List<ColumnLsts> ColumnLists { get; set; }
    }
}

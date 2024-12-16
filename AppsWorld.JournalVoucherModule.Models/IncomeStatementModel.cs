using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
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
    public class ColumnLsts
    {
        public string Column { get; set; }
        public string HtmlData { get; set; }
        public bool? IsAmount { get; set; }
    }





    public class NewStatementModel
    {
        public List<ColumnLsts> ColumnLists { get; set; }
        public List<AccountNewModel> ListAccountNewModel { get; set; }


    }
    public class AccountNewModel
    {
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string Type { get; set; }
        public string GroupType { get; set; }
        public string GroupHeading { get; set; }
        public string Class { get; set; }
        public bool IsShowZero { get; set; }
        public int? Recorder { get; set; }
        public bool? IsInterco { get; set; }
        public List<YearModels> YearModels { get; set; }
    }







}

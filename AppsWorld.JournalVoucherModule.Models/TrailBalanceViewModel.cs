using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{

    public class TrailBalanceModel
    {
        public List<string> ColumnList { get; set; }
        public List<TrailBalanceViewModel> TrailBalanceViewModel { get; set; }
        public List<YearModel> TrailBalanceTotals { get; set; }
    }
    public class TrailBalanceViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public List<YearModel> YearModel { get; set; }
    }


    public class YearModels
    {
        public string Year { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Percentage { get; set; }
        public bool IsPercentage { get; set; } = false;
        public string FontColor { get; set; }

    }
    public class YearModel
    {
        public string Year { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Percentage { get; set; }
        public bool IsPercentage { get; set; } = true;
        public string FontColor { get; set; }
    }

    public class TrailBalanceSpModel
    {
        public long? Recorder { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Year { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string Percentage { get; set; }
    }
    public class BalanceSheetSpModel
    {
        public long? Recorder { get; set; }
        public Guid FRCoaId { get; set; }
        public Guid FRPATId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Percentage { get; set; }
        public Nullable<int> FRRecOrder { get; set; }
        public long? CoaId { get; set; }
    }

    public class SaveCategoryModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Guid AccountTypeId { get; set; }
        public string Name { get; set; }
        public bool IsCategory { get; set; }
        public bool IsIncomeStatement { get; set; }
        public string RecordStatus { get; set; }
        public List<long> AccountIds { get; set; }
    }
}

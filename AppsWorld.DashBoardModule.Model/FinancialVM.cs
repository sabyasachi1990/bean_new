using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models
{
 public   class FinancialVM
    {
        public DateTime FromDate { get; set; }
        public string ToDate { get; set; }
        public BasicColumnDrillDownVM GetFinancialDashBoard { get; set; }
        public virtual List<FinancialScoreCardModel> FinancialScoreCardModels { get; set; }

    }
    public class FinancialData
    {
        public string SubCategory { get; set; }
        public string Class { get; set; }
        public string MonthYear { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
    }
    public class FinancialScoreCardModel
    {
        public string Name { get; set; }
        public double CurrentMonthAmount { get; set; }
        public double PreviousMonthAmount { get; set; }
        public double DifferanceMonthAmount { get; set; }
        public double CurrentYearAmount { get; set; }
        public double PreviousYearAmount { get; set; }
      

    }
}

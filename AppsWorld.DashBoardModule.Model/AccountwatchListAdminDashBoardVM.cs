using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Model
{
    public class AccountwatchListAdminDashBoardVM
    {
        public DateTime FromDate { get; set; }
        public string ToDate { get; set; }
        public AccountwatchListKPI AccountWatchListKPI { get; set; }
        public MultiDrillDownVM AccountWatchListBalanceSheet { get; set; }
        public MultiDrillDownVM AccountWatchListProfitandLoss { get; set; }
    }
    public class AccountwatchListKPI
    {
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal NetProfit { get; set; }
    }
    public class AccountwatchListAdminData
    {
        public decimal Value { get; set; }
        public string MonthYear { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChartOfList { get; set; }
    }
    
}

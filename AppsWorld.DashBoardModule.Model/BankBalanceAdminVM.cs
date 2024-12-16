using AppsWorld.DashBoardModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppsWorld.DashBoardModule.Model
{
    public class BankBalanceAdminVM
    {   
        public DateTime fromDate { get; set; }
        public string toDate { get; set; }
        public BankBalanceKPI BankBalanceKPI { get; set; }
        public DrillDownChartVM BankBalanceDrillDownChart { get; set; }
        public BasicColumnDrillDownVM BankTotalInandOutBasicColumnDrillDown { get; set; }
    }
    public class BankBalanceKPI
    {
        public decimal In { get; set; }
        public decimal Out { get; set; }
        public decimal Balance { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models
{
  public  class BankDetailsDashBoardVM
    {
        public DrillDownChartVM BankBalanceDrillDownChart { get; set; }
        public BasicColumnDrillDownVM ChangeMonthlyDrillDownChart { get; set; }
        public virtual List<BankDashBoardGrid> BankDashBoardGrid { get; set; }
        public string ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
    public class BankDetailData
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string MonthYear { get; set; }
    }
    public class BankDetailDashBoardModel
    {
        public string Name { get; set; }
        public double Inflow { get; set; }
        public double Outflow { get; set; }
        public double Differance { get; set; }
        public string MonthYear { get; set; }
    }
    public class BankDashBoardGrid
    {   
        public string Name { get; set; }   
        public string BDBG_1_10 { get; set; }    
        public string BDBG_11_30 { get; set; }      
        public string BDBG_31_60 { get; set; }  
        public string BDBG_61_More { get; set; }
    }
    
}

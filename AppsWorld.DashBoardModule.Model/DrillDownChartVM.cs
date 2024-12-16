using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models
{
   public class DrillDownChartVM
    {
        public DrillDownChartVM()
        {  
            MainData = new List<DrillDownMainData1>();
            SubData = new List<DrillDownSubData1>();           
        }
        public List<DrillDownMainData1> MainData { get; set; }
        public List<DrillDownSubData1> SubData { get; set; }
    }
    public class DrillDownChartVM1
    {
        public DrillDownChartVM1()
        {
            MainData = new List<MainColumnData>();
            SubData = new List<DrillDownSubData1>();
        }
        public List<MainColumnData> MainData { get; set; }
        public List<DrillDownSubData1> SubData { get; set; }
    }
    public class MainColumnData
    {
        public string name { get; set; }
        public List<DrillDownMainData1> data { get; set; }
    }

    public class DrillDownMainData1
    {
        public string name { get; set; }
        public double y { get; set; }
        //public string color { get; set; }
        public string drilldown { get; set; }
        //public string MonthYear { get; set; }
        //public string TooltipValue { get; set; }

    }

    public class DrillDownSubData1
    {
        public DrillDownSubData1()
        {
            data = new List<List<string>>();
        }
        public string type { get; set; }
        public string name { get; set; }
        public string id { get; set; }
       // public string TooltipValue { get; set; }
        public List<List<string>> data { get; set; }

    }
    public class SubDtata1
    {
        public string SubName { get; set; }

        public int SubValue { get; set; }

        public double Amount { get; set; }

        public string MonthYear { get; set; }
    }
    public class BankBalanceData
    {
        public double Balance { get; set; }
        public string Monthyear { get; set; }
        public DateTime Date { get; set; }
        public double IN { get; set; }
        public double Out { get; set; }
        public double Diff { get; set; }
    }
}

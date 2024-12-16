using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Model
{
    public class MultiDrillDownVM
    {
        public MultiDrillDownVM()
        {
            MainData = new List<MultiDrillDownMainData>();
            SubData = new List<MultiDrillDownSubData>();
        }
        public string[] DropDownValues { get; set; }
        public List<MultiDrillDownMainData> MainData { get; set; }
        public List<MultiDrillDownSubData> SubData { get; set; }
    }
    public class MultiDrillDownMainData
    {
        public string name { get; set; }
        public decimal y { get; set; }
        public string color { get; set; }
        public string drilldown { get; set; }
    }

    public class MultiDrillDownSubData
    {
        public string name { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public List<MultiDrillDownSeriesData> data { get; set; }
    }
    public class MultiDrillDownSeriesData
    {
        public string name { get; set; }
        public decimal y { get; set; }
        public string drilldown { get; set; }
    }
    public class ProfitAndLossData
    {
        public string MonthYear { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount1 { get; set; }
        public string Date { get; set; }
    }
}


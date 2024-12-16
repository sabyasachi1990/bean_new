using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models
{
    public  class BasicColumnDrillDownVM
    {
        public BasicColumnDrillDownVM()
        {
            MainData = new List<DrillDownMainData>();
            SubData = new List<DrillDownSubData>();
        }
        public List<DrillDownMainData> MainData { get; set; }
        public List<DrillDownSubData> SubData { get; set; }
    }
    public class DrillDownMainData
    {
        public string name { get; set; }
        public string color { get; set; }
        public List<DrillDownSeriesData> data { get; set; }
    }
    public class DrillDownSeriesData
    {
        public string name { get; set; }
        public double y { get; set; }
        public string drilldown { get; set; }
        public double IN { get; set; }
        public double Out { get; set; }
        public double Diff { get; set; }
    }
    public class DrillDownSubData
    {
        public DrillDownSubData()
        {
            data = new List<List<string>>();
        }
        public string type { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public List<List<string>> data { get; set; }
    }
    public class SubDtata
    {
        public string SubName { get; set; }
        public int SubValue { get; set; }
        public double Amount { get; set; }
        public string MonthYear { get; set; }
    }
    public class BasicColumnData
    {
        public double In { get; set; }
        public double Out { get; set; }
        public double Balance { get; set; }                         
        public string MonthYear { get; set; }
        public DateTime Date { get; set; }
    }
    //public class BasicData
    //{
    //    public string monthyear { get; set; }
    //    public double amount { get; set; }
    //    public DateTime date { get; set; }
    //}
}

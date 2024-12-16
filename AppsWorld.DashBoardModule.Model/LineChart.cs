using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models
{
  public  class LineChart
    {
        public LineChart()
        {
            LineData = new List<LineChartData>();
        }

        public List<LineChartData> LineData { get; set; }
        public List<string> Categories { get; set; }
    }
    public class LineChartData
    {
        public string name { set; get; }
        public List<double> data { set; get; }
        public List<string> ToolTipValue { get; set; }
    }
}

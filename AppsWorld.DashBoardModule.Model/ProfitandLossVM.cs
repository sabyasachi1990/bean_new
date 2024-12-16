using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;                   

namespace AppsWorld.DashBoardModule.Model
{
    public class ProfitandLossVM
    {
        public DateTime fromDate { get; set; }
        public string toDate { get; set; }
        public MultiDrillDownVM BasicDrillDown { get; set; }
    }
}
  
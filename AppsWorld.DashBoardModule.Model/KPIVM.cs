using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models
{
    public class KPIVM
    {
        public DateTime FromDate { get; set; }
        public string ToDate { get; set; }
        public List<KPINetData> KPIVMRatio { get; set; }
    }
    public class KPINetData
    {
        public string NetAssetRatio1 { get; set; }
        public string NetAssetRation2 { get; set; }
        public string WorkingCapitalRatio1 { get; set; }
        public string WorkingCapitalRatio2 { get; set; }
        public string DebtEquityRatio1 { get; set; }
        public string DebtEquityRatio2 { get; set; }
        public int ReturnOnEquity { get; set; }
        public int ProfitorLoss { get; set; }
     
     }
}

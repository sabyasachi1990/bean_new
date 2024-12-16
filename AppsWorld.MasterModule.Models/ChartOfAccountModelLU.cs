using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public partial class ChartOfAccountModelLU
    {
        public LookUpCategory<string> CashFlowTypeLU { get; set; }
        //public LookUpCategory<string> AllowableNonalowablLU { get; set; }
        public bool IsMultiCurrencyActivated { get; set; }
        public bool IsAllowableNotAllowableActivated { get; set; }
        public bool? IsRevaluationActivated { get; set; }
        public bool IsSeedData { get; set; }
        public List<CurrencyLU> CurrencyLU { get; set; }
    }
}

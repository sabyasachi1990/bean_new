using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class BalanceSheetModel
    {
        public string PriorYear { get; set; }
        public string CurrentYear { get; set; }
        public bool? IsAssets { get; set; }
        public LeadSheetTypeModel Assets { get; set; }
        public AccountModel AssetsTotal { get; set; }
        public bool? IsEquity { get; set; }
        public LeadSheetTypeModel Equity { get; set; }
        public AccountModel EquityTotal { get; set; }
        public bool? IsLiabilities { get; set; }
        public LeadSheetTypeModel Liabilities { get; set; }
        public AccountModel LiabilitiesTotal { get; set; }
        public AccountModel LiabilitiesEquityTotal { get; set; }
        public bool? IsFirstEngagement { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class OBAmountModel
    {
        public decimal? DocDebitTotal { get; set; }
        public decimal? DocCreditTotal { get; set; }
        public decimal? BaseDebitTotal { get; set; }
        public decimal? BaseCreditTotal { get; set; }
        public string Currency { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class FinancialSummaryModel
    {
        public decimal? BillingsAmount { get; set; }
        public decimal? ReceiptsAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? DebtProvAmount { get; set; }
        public decimal? NetBalAmount { get; set; }
    }
}

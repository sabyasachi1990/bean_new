using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class UpdatePosting
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocumentState { get; set; }
        public decimal? BalanceAmount { get; set; }
        public Nullable<decimal> AllocatedAmount { get; set; }
    }
}

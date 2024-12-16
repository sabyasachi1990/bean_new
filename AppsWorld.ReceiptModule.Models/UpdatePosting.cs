using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Models
{
    public class UpdatePosting
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocumentState { get; set; }
        public decimal? BalanceAmount { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

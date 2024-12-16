using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Models
{
    public class GSTDetailModel
    {
        public System.Guid Id { get; set; }
        public string DocType { get; set; }
        public System.Guid DocId { get; set; }
        public long ModuleMasterId { get; set; }
        public decimal Amount { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public Nullable<long> TaxId { get; set; }
        public string RecordStatus;
    }
}

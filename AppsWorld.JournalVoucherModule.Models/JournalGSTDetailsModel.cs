using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JournalGSTDetailsModel
    {
		public System.Guid Id { get; set; }
		public System.Guid JournalId { get; set; }
		public long TaxId { get; set; }
		public decimal Amount { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal TotalAmount { get; set; }
		public string  TaxCode { get; set; }
    }
}

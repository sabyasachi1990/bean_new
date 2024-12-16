using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class TaxModel
    {
        public long? TaxId { get; set; }
        public string TaxCode { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string TaxIdCode { get; set; }
    }
}

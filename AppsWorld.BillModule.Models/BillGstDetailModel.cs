using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
  public   class BillGstDetailModel
    {
        public Guid Id { get; set; }
        public Guid BillId { get; set; }
        public long TaxId { get; set; }
        public string TaxCode { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        //public virtual Bill Bill { get; set; }
       // public virtual TaxCode TaxCode1 { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
   public class GetCustomerLU
    {
        public string Code { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
        public long? TOPId { get; set; }
        public string Nature { get; set; }

        public decimal CustCreditLimit { get; set; }

    }
}

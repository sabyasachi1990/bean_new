using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class BillPaymentModel
    {
        public System.Guid PaymentDetailId { get; set; }

        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }

        public string SystemRefNo { get; set; }

        public decimal? Ammount { get; set; }
        public string DocType { get; set; }
        //public virtual ICollection<BillMemoModel> BillMemoModels { get; set; }

    }
}

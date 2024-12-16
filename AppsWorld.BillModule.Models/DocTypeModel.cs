using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class DocTypeModel
    {
        public List<VendorDocumentDetailModel> BillDetailModels { get; set; }
        public decimal? PaymentTotalAmount { get; set; }
        public decimal? CreditMemoTotalAmount { get; set; }
        public decimal? AmountDue { get; set; }
    }
}

using AppsWorld.ReceiptModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Models
{
   public class ReceiptsModel
    {
        public Guid Id { get; set; }
      

        public string DocNo { get; set; }
        public string PaymentMode { get; set; }

        public string DocCurrency { get; set; }

        public decimal Total { get; set; }

        public Nullable<decimal> ReceiptApplicationAmmount { get; set; }


        public string PaymentAmount{ get; set; }
        public string RecieptNo { get; set; }
        public string UserCreated { get; set; }

    
        public DateTime? RecieptDate { get; set; }

        //public List<ReceiptDetaislModel> DetailModel { get; set; }

        

    }
  
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models.V2
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

    
        public String RecieptDate { get; set; }


        public List<ReceiptDetaislModel> ReceiptDetaislModel { get; set; }

        

    }
    public class ReceiptDetaislModel
    {
        public string DocType { get; set; }
     
        public string DocNo { get; set; }

        public string Currency { get; set; }
        public decimal? DocTot { get; set; }

        public decimal? GrandTotal { get; set; }

        public decimal? DocBalance { get; set; }

        public decimal PaymentAmt { get; set; }




    }
}

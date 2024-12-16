//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppsWorld.TemplateModule.Models.V2
//{
//    public class TemplateMenuModel
//    {
//        //public TemplateMenuModel()
//        //{
//        //    this.InvoiceModel = new List<Models.InvoiceModel>();
//        //}
//        public Invoice Invoice { get; set; }
//        public Entitys Entity { get; set; }
//        public CompanyModel ServiceEntity { get; set; }
//        public Nullable<decimal> TotalOutstandingBalanceFee { get; set; }

    
//    }

//    public class Invoice
//    {
//        public string DocNo { get; set; }
//        public string DocDate { get; set; }
//        public  string CreditTerms { get; set; }
//        public string DueDate { get; set; }
//        public string PoNo { get; set; }
//        public string Currency { get; set; }
//        public Decimal? ExchangeRate { get; set; }
//        public string DocumentDescription { get; set; }
//        public List<Items> Item { get; set; }
//    }
//    public class Entitys
//    {
//        public string EntityName { get; set; }
//        public string RegisteredAddress { get; set; }
//        public string MailingAddress { get; set; }
//    }

//    public class Items
//    {
//        public string ItemCode { get; set; }
//        public string ItemDescription { get; set; }
//        public string Quantity { get; set; }
//        public string Unit { get; set; }
//        public string UnitPrice { get; set; }
//        public string Amount { get; set; }
//        public string TaxAmount { get; set; }
//        public string Total { get; set; }
//        public string Discount { get; set; }
//    }
//}

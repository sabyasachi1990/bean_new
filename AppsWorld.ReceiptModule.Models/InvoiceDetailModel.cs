using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.ReceiptModule.Models
{
    //using Repository.Pattern.Ef6;
    //using System;
    //using System.Collections.Generic;
    //using System.ComponentModel.DataAnnotations;
    //using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    // [Table("Bean.InvoiceDetail")]
    public  class InvoiceDetailModel 
    {
        public InvoiceDetailModel()
        {
            //this.Item = new Item();
           // this.TaxCode = new TaxCode();
            //this.ChartOfAccount = new ChartOfAccount();
        }
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public string AmtCurrency { get; set; }
        public string AccountName { get; set; }

        public string ItemDescription { get; set; }
     
        public long COAId { get; set; }
      
        public Nullable<long> TaxId { get; set; }
        public decimal? DocTotalAmount { get; set; }
        public decimal? DocAmount { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        public double Qty { get; set; }

        public string TaxIdCode { get; set; }
      
    }
}

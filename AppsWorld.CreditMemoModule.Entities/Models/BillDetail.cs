using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;


namespace AppsWorld.CreditMemoModule.Entities
{
    public partial class BillDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid BillId { get; set; }
        //public string Account { get; set; }
        //public string Description { get; set; }
        //public long COAId { get; set; }
        //public bool IsDisallow { get; set; }
        //public Nullable<long> TaxId { get; set; }

        //public string TaxCode { get; set; }
        //public string TaxType { get; set; }
        //public Nullable<double> TaxRate { get; set; }
        public decimal DocAmount { get; set; }
        //public decimal DocTaxAmount { get; set; }
        //public decimal DocTotalAmount { get; set; }
        //public Nullable<decimal> BaseAmount { get; set; }
        //public Nullable<decimal> BaseTaxAmount { get; set; }
        //public Nullable<decimal> BaseTotalAmount { get; set; }
        //public int? RecOrder { get; set; }
        [NotMapped]
        public string RecordStatus;
        //public bool? IsPLAccount { get; set; }
		
    }
}

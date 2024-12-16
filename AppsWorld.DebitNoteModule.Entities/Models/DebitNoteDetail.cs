using AppsWorld.CommonModule.Entities;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.DebitNoteModule.Entities
{
    public partial class DebitNoteDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid DebitNoteId { get; set; }
        public long COAId { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public string TaxType { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public decimal DocAmount { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public Nullable<decimal> BaseAmount { get; set; }
        public Nullable<decimal> BaseTaxAmount { get; set; }
        public Nullable<decimal> BaseTotalAmount { get; set; }
        public int? RecOrder { get; set; }
        public string AccountDescription { get; set; }
        public bool? IsPLAccount { get; set; }

        [NotMapped]
        public string RecordStatus;
        public string TaxIdCode { get; set; }
        public string ClearingState { get; set; }
        //[NotMapped]
        //public string AccountName;

        //[NotMapped]
        //public string TaxIdCode;




        //[ForeignKey("COAId")]
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        ////public virtual DebitNote DebitNote { get; set; }
        //[ForeignKey("TaxId")]
        //public virtual TaxCode TaxCodes { get; set; }
        //[NotMapped]
        //public string TaxCode { get; set; }
    }
}

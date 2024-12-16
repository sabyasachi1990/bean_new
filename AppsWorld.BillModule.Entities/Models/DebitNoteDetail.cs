using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.BillModule.Entities
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

        [NotMapped]
        public string RecordStatus;

        [NotMapped]
        public string TaxIdCode;
        [ForeignKey("COAId")]
        public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual DebitNote DebitNote { get; set; }
         [ForeignKey("TaxId")]
        public virtual TaxCode TaxCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using AppsWorld.JournalVoucherModule.Entities;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.JournalVoucherModule.Models
{
    public partial class JournalGSTDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid JournalId { get; set; }
        public Nullable<long> TaxId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        //public virtual Journal Journal { get; set; }
        [NotMapped]
        public string RecordStatus;
		public virtual TaxCode TaxCode { get; set; }
        //public virtual List<JournalGSTTaxDetail> TaxDetails { get; set; }
    }
}

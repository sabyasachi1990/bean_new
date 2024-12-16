using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class GLClearingDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid GLClearingId { get; set; }
        public string DocType { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string SystemRefNo { get; set; }
        public decimal DocAmount { get; set; }
        public string DocCurrency { get; set; }
        public decimal BaseAmount { get; set; }
        public string BaseCurrency { get; set; }
        public string CrDr { get; set; }
        public bool? IsCheck { get; set; }
        //[ForeignKey("GLClearingId")]
        //public virtual GLClearing GLClearing { get; set; }
        public int? RecOrder { get; set; }
    }
}

using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Entities
{
    public partial class DoubtfulDebtAllocationDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid DoubtfulDebtAllocationId { get; set; }
        public System.Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocCurrency { get; set; }
        public decimal AllocateAmount { get; set; }
        public string DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public Guid? EntityId { get; set; }
        public decimal? ExchangeRate { get; set; }

        //public virtual DoubtfulDebtAllocation DoubtfulDebtAllocation { get; set; }
    }
}

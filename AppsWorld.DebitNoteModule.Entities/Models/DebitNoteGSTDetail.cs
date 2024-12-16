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
    //public partial class DebitNoteGSTDetail : Entity
    //{
    //   public System.Guid Id { get; set; }
    //    public System.Guid DebitNoteId { get; set; }
    //    public long TaxId { get; set; }
    //    public decimal Amount { get; set; }
    //    public decimal TaxAmount { get; set; }
    //    public decimal TotalAmount { get; set; }
    //    //public virtual DebitNote DebitNote { get; set; }
    //    [ForeignKey("TaxId")]
    //    public virtual TaxCode TaxCode { get; set; }

    //    [NotMapped]
    //    public string RecordStatus;
    //}
}

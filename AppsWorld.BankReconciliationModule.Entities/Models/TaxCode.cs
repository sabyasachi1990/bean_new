using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.BankReconciliationModule.Entities;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class TaxCode:Entity
    {
        public TaxCode()
        {
            //this.BillCreditMemoGSTDetails = new List<BillCreditMemoGSTDetail>();
            //this.BillDetails = new List<BillDetail>();
            //this.BillGSTDetails = new List<BillGSTDetail>();
         //   this.DebitNoteDetails = new List<DebitNoteDetail>();
            //this.DebitNoteGSTDetails = new List<DebitNoteGSTDetail>();
            //this.InvoiceDetails = new List<InvoiceDetail>();
            //this.InvoiceGSTDetails = new List<InvoiceGSTDetail>();
         //   this.Items = new List<Item>();
            this.ReceiptBalancingItems = new List<ReceiptBalancingItem>();
            //this.ReceiptGSTDetails = new List<ReceiptGSTDetail>();
        }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AppliesTo { get; set; }
        public string TaxType { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public bool IsSystem { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }        public string TaxRateFormula { get; set; }
        public Nullable<bool> IsApplicable { get; set; }
        //public virtual ICollection<BillCreditMemoGSTDetail> BillCreditMemoGSTDetails { get; set; }
        //public virtual ICollection<BillDetail> BillDetails { get; set; }
        //public virtual ICollection<BillGSTDetail> BillGSTDetails { get; set; }
        //public virtual ICollection<DebitNoteDetail> DebitNoteDetails { get; set; }
        //public virtual ICollection<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }
        //public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        //public virtual ICollection<InvoiceGSTDetail> InvoiceGSTDetails { get; set; }
      //  public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<ReceiptBalancingItem> ReceiptBalancingItems { get; set; }
        //public virtual ICollection<ReceiptGSTDetail> ReceiptGSTDetails { get; set; }
        public virtual Company Company { get; set; }
    }
}

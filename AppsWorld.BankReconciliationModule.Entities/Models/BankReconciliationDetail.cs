using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.BankReconciliationModule.Entities;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class BankReconciliationDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid BankReconciliationId { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string DocRefNo { get; set; }
        //public string RefernceNo { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public Nullable<decimal> Ammount { get; set; }
        public string ClearingStatus { get; set; }
        public System.Guid DocumentId { get; set; }

        //public string EntityName { get; set; }
        // public virtual BankReconciliation BankReconciliation { get; set; }
        public Nullable<bool> isWithdrawl { get; set; }
        public bool? IsDisable { get; set; }
        public bool? IsChecked { get; set; }
        public string Mode { get; set; }
        public string RefNo { get; set; }
        public string DocSubType { get; set; }
        public Guid? JournalDetailId { get; set; }

        //[ForeignKey("EntityId")]
        //public virtual BeanEntity BeanEntity { get; set; }
    }
}

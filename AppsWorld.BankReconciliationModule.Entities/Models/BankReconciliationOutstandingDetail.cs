using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.BankReconciliationModule.Entities;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class BankReconciliationOutstandingDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid BankReconciliationId { get; set; }
        public Nullable<System.Guid> ReceiptId { get; set; }
        public Nullable<System.Guid> PaymentId { get; set; }
        public bool IsDeposit { get; set; }
        public virtual BankReconciliation BankReconciliation { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}

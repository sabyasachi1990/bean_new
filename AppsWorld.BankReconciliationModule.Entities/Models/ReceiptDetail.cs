using System;
using System.Collections.Generic;
using AppsWorld.BankReconciliationModule.Entities;
using System.Data.Entity;
using AppsWorld.BankReconciliationModule.Entities.Models;
using Repository.Pattern.Ef6;
using AppsWorld.BankReconciliationModule.RepositoryPattern;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class ReceiptDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid ReceiptId { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public string DocumentType { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentState { get; set; }
        public string Nature { get; set; }
        public decimal DocumentAmmount { get; set; }
        public decimal AmmountDue { get; set; }
        public string Currency { get; set; }
        public decimal ReceiptAmount { get; set; }
        public System.Guid DocumentId { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.BankReconciliationModule.Entities;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class BankReconciliation : Entity
    {
        public BankReconciliation()
        {
            this.BankReconciliationDetails = new List<BankReconciliationDetail>();

        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public long COAId { get; set; }
        public long ServiceCompanyId { get; set; }
        public System.DateTime BankReconciliationDate { get; set; }
        public string Currency { get; set; }
        public string BankAccount { get; set; }
        public decimal StatementAmount { get; set; }
        //public decimal OutstandingWithdrawals { get; set; }
        public decimal SubTotal { get; set; }
        //public decimal OutstandingDeposits { get; set; }
        public decimal StatementExpectedAmount { get; set; }
        public decimal GLAmount { get; set; }
        public string State { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<System.DateTime> StatementDate { get; set; }
        public Nullable<Boolean> IsDraft { get; set; }
        public bool? IsReRunBR { get; set; }
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
        }
        public bool? IsLocked { get; set; }
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<BankReconciliationDetail> BankReconciliationDetails { get; set; }

    }
}

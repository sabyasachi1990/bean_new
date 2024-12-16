using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
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
    public partial class DoubtfulDebtAllocation : Entity
    {
        public DoubtfulDebtAllocation()
        {
            this.DoubtfulDebtAllocationDetails = new List<DoubtfulDebtAllocationDetail>();
        }

        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DoubtfulDebtAllocationDate { get; set; }
        public Nullable<System.DateTime> DoubtfulDebtAllocationResetDate { get; set; }
        public bool IsNoSupportingDocumentActivated { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public decimal AllocateAmount { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public bool? IsRevExcess { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool? IsLocked { get; set; }

        DoubtfulDebtAllocationStatus _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public DoubtfulDebtAllocationStatus Status
        {
            get
            {
                return _status;
            }
            set { _status = (DoubtfulDebtAllocationStatus)value; }
        }
        public string DoubtfulDebtAllocationNumber { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<DoubtfulDebtAllocationDetail> DoubtfulDebtAllocationDetails { get; set; }
    }
}

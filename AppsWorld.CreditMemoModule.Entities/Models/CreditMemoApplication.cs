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

namespace AppsWorld.CreditMemoModule.Entities
{
    public partial class CreditMemoApplication:Entity
    {
        public CreditMemoApplication()
        {
            this.CreditMemoApplicationDetails = new List<CreditMemoApplicationDetail>();
        }

        public System.Guid Id { get; set; }
        public System.Guid CreditMemoId { get; set; }
        public long CompanyId { get; set; }
        public int? ClearCount { get; set; }
        public System.DateTime CreditMemoApplicationDate { get; set; }
        public Nullable<System.DateTime> CreditMemoApplicationResetDate { get; set; }
        public bool IsNoSupportingDocumentActivated { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public decimal CreditAmount { get; set; }
        public string Remarks { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public string CreditMemoApplicationNumber { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Guid? DocumentId { get; set; }
        CreditMemoApplicationStatus _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public CreditMemoApplicationStatus Status
        {
            get
            {
                return _status;
            }
            set { _status = (CreditMemoApplicationStatus)value; }
        }
        public Nullable<decimal> ExchangeRate { get; set; }
        public bool? IsRevExcess { get; set; }
        public string ClearingState { get; set; }
        public bool? IsLocked { get; set; }
        //[ForeignKey("CreditMemoId")]
        //public virtual CreditMemo CreditMemo { get; set; }
        //[ForeignKey("CompanyId")]
        //public virtual Company Company { get; set; }
        public virtual ICollection<CreditMemoApplicationDetail> CreditMemoApplicationDetails { get; set; }
        public decimal? RoundingAmount { get; set; }
    }
}

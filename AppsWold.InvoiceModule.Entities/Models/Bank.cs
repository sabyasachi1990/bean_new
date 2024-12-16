using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Entities
{
    public partial class Bank : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<long> SubcidaryCompanyId { get; set; }
        public string ShortCode { get; set; }
        public string Name { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string SwiftCode { get; set; }
        public Nullable<System.Guid> AddressBookId { get; set; }
        public string BankAddress { get; set; }
        public Nullable<long> COAId { get; set; }
        public string Currency { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        // public Nullable<int> Status { get; set; }
        public string ShortName { get; set; }

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
        // [ForeignKey("COAId")]
        //   public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual AddressBook AddressBook { get; set; }
        //public virtual Company Company { get; set; }
        public string Purpose { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class TermsOfPayment : Entity
    {
        public TermsOfPayment()
        {
            this.Invoices = new List<Invoice>();
            this.DebitNotes = new List<DebitNote>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public long CompanyId { get; set; }

        [StringLength(20)]
        public string TermsType { get; set; }

        public Nullable<double> TOPValue { get; set; }

        public Nullable<int> RecOrder { get; set; }

        [StringLength(256)]
        public string Remarks { get; set; }

        [StringLength(254)]
        public string UserCreated { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        [StringLength(254)]
        public string ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<short> Version { get; set; }

        public bool? IsVendor { get; set; }
        public bool? IsCustomer { get; set; }

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
        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<DebitNote> DebitNotes { get; set; }
    }
}

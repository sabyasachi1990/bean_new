using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    public partial class TermsOfPayment : Entity
    {
        public TermsOfPayment()
        {
            //this.Invoices = new List<Invoice>();
            //this.DebitNotes = new List<DebitNote>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public long CompanyId { get; set; }

        public string TermsType { get; set; }

        public Nullable<double> TOPValue { get; set; }

        public Nullable<int> RecOrder { get; set; }

        public string Remarks { get; set; }

        public string UserCreated { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

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
        //public virtual ICollection<Invoice> Invoices { get; set; }

        //public virtual ICollection<DebitNote> DebitNotes { get; set; }
    }
}

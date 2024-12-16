using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public class Contact : Entity
    {
        public Contact()
        {
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.Guid> PhotoId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string IdType { get; set; }
        public string IdNo { get; set; }
        public string CountryOfResidence { get; set; }
        public Nullable<System.Guid> MailingAddressBookId { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
     
        public Nullable<short> Version { get; set; }
        RecordStatusEnum _status;

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (RecordStatusEnum)value; }
        }
        public virtual Company Company { get; set; }
        public virtual ICollection<ContactDetail> ContactDetails { get; set; }

        public string Communication { get; set; } // Added By Pavan

    }
}

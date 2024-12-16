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
    public class AddressBook : Entity
    {
        public AddressBook()
        {
            //this.UserAccounts = new List<UserAccount>();
            //this.Entities = new List<Entity>();
            //this.Accounts = new List<Account>();
            //this.Vendors = new List<Vendor>();
            //this.Addresses = new List<Address1>();
            //this.Banks = new List<Bank>();
            //this.Companies = new List<Company>();
            //this.Contacts = new List<Contact>();
        }

        public System.Guid Id { get; set; }
        public Nullable<bool> IsLocal { get; set; }
        public string BlockHouseNo { get; set; }
        public string Street { get; set; }
        public string UnitNo { get; set; }
        public string BuildingEstate { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
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

        public Guid? DocumentId { get; set; }

        //public Nullable<int> Status { get; set; }

        //public virtual ICollection<UserAccount> UserAccounts { get; set; }
        //public virtual ICollection<Entity> Entities { get; set; }
        //public virtual ICollection<Account> Accounts { get; set; }
        //public virtual ICollection<Vendor> Vendors { get; set; }
        //public virtual ICollection<Address1> Addresses { get; set; }
        //public virtual ICollection<Bank> Banks { get; set; }
        //public virtual ICollection<Company> Companies { get; set; }
        //public virtual ICollection<Contact> Contacts { get; set; }
    }
}

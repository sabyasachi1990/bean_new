using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.InvoiceModule.Entities.Models
{
    public partial class AddressBook : Entity
    {
        public AddressBook()
        {
            //this.UserAccounts = new List<UserAccount>();
            //this.Entities = new List<BeanEntity>();
            //this.Accounts = new List<Account>();
            //this.Vendors = new List<Vendor>();
            //this.Companies = new List<Company>();
            //this.Contacts = new List<Contact>();
        }

        public Guid Id { get; set; }
        public bool? IsLocal { get; set; }

        [StringLength(256)]
        public string BlockHouseNo { get; set; }

        [StringLength(256)]
        public string Street { get; set; }

        [StringLength(256)]
        public string UnitNo { get; set; }

        [StringLength(256)]
        public string BuildingEstate { get; set; }

        [StringLength(256)]
        public string City { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(256)]
        public string State { get; set; }

        [StringLength(256)]
        public string Country { get; set; }

        [StringLength(1000)]
        public string Phone { get; set; }

        [StringLength(1000)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Website { get; set; }

        [Column(TypeName = "money")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "money")]
        public decimal? Longitude { get; set; }
        public int? RecOrder { get; set; }

        [StringLength(256)]
        public string Remarks { get; set; }

        [StringLength(254)]
        public string UserCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(254)]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDate { get; set; }
        public short? Version { get; set; }

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
        //public virtual ICollection<UserAccount> UserAccounts { get; set; }
        //public virtual ICollection<BeanEntity> Entities { get; set; }
        //public virtual ICollection<Account> Accounts { get; set; }
        //public virtual ICollection<Vendor> Vendors { get; set; }
        //public virtual ICollection<Company> Companies { get; set; }
        //public virtual ICollection<Contact> Contacts { get; set; }
    }
}

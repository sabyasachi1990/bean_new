using System;
using System.Collections.Generic;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AppsWorld.MasterModule.Entities;
using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.MasterModule.Models
{
    public class ContactModel
    {
        public ContactModel()
        {
            this.Addresses = new List<Address>();
            this.ContactDetailModel = new ContactDetailModel();
        }
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public long? CompanyId { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string IdType { get; set; }
        public string IdNo { get; set; }
        public string CountryOfResidence { get; set; }
        public Guid? PhotoId { get; set; }
        public string Designation { get; set; }
        public string Communication { get; set; }
        public Guid EntityId { get; set; }

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
            set
            {
                if (value != null)
                    _status = value;
            }
        }

        public string SourceType { get; set; }
        public string MediaType { get; set; }
        public string Original { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Large { get; set; }
        public System.Guid ClientId { get; set; }
        public List<Address> Addresses { get; set; }
        public string UserCreated { get; set; }
        public ContactDetailModel ContactDetailModel { get; set; }
        public bool IsAssociate { get; set; }
        
    }
}

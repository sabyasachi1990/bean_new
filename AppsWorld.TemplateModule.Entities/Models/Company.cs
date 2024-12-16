using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using FrameWork;

namespace AppsWorld.TemplateModule.Entities.Models
{
    public partial class Company : Entity
    {
        public Company()
        {
           
            this.Bank = new List<Bank>();
           
        }

        public long Id { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string RegistrationNo { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> LogoId { get; set; }
        public string CssSprite { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<System.Guid> AddressBookId { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public bool? IsGstSetting { get; set; }
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
        public string ShortName { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Communication { get; set; }

       
        public ICollection<Bank> Bank { get; set; }
        


    }
}

using AppsWorld.Framework;

using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class Address : Entity
    {
        public System.Guid Id { get; set; }
        public string AddSectionType { get; set; }
        public string AddType { get; set; }
        public Nullable<System.Guid> AddTypeId { get; set; }
        public Nullable<long> AddTypeIdInt { get; set; }
        public Nullable<System.Guid> AddressBookId { get; set; }

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
        public virtual AddressBook AddressBook { get; set; }
    }
}

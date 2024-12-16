using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class UserAccount : Entity
    {
        public UserAccount()
        {
            //this.UserPermissions = new List<UserPermission>();
        }

        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.Guid> PhotoId { get; set; }

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
        public string Title { get; set; }
        public string Gender { get; set; }
        public string PhoneNo { get; set; }
        public Nullable<System.DateTime> DeactivationDate { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public string Salutation { get; set; }

        public string Communication { get; set; }


        //public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}

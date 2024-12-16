using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public partial class ContactDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid ContactId { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string EntityType { get; set; }
        public string Designation { get; set; }
        public string Communication { get; set; }
        public string Matters { get; set; }
        public Nullable<bool> IsPrimaryContact { get; set; }
        public Nullable<bool> IsReminderReceipient { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string OtherDesignation { get; set; }
        public Nullable<bool> IsPinned { get; set; }
        public string CursorShortCode { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public Nullable<int> Status { get; set; }
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (RecordStatusEnum)value; }
        }
        public virtual Contact Contact { get; set; }
    }
}

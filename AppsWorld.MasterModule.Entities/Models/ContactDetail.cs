using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.MasterModule.Entities
{
    public partial class ContactDetail : Entity
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Guid? EntityId { get; set; }
        public string EntityType { get; set; }
        public string Designation { get; set; }
        public string Communication { get; set; }
        public string Matters { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? IsReminderReceipient { get; set; }
        public int? RecOrder { get; set; }
        public string OtherDesignation { get; set; }
        public bool? IsPinned { get; set; }
        public bool? IsCopy { get; set; }
        public string CursorShortCode { get; set; }
        public Guid? DocId { get; set; }//for Wf+CC
        public string DocType { get; set; }//for Wf+CC
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
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

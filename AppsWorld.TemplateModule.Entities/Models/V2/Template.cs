using System;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using FrameWork;
namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public partial class Template : Entity
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long CompanyId { get; set; }
        public string FromEmailId { get; set; }
        public string CCEmailIds { get; set; }
        public string BCCEmailIds { get; set; }
        public string TemplateType { get; set; }
        public string TempletContent { get; set; }
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
        public string Subject { get; set; }
        public string TemplateMenu { get; set; }
        public string ToEmailId { get; set; }
        public Nullable<bool> IsUnique { get; set; }
        public string CursorName { get; set; }
    }
}

using AppsWorld.Framework;
using AppsWorld.InvoiceModule.Entities.V2;
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

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public partial class GenericTemplateCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.Guid> TemplateTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TempletContent { get; set; }
        public Nullable<bool> IsFooterExist { get; set; }
        public Nullable<bool> IsHeaderExist { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
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
        public string CCEmailIds { get; set; }
        public string BCCEmailIds { get; set; }
        public string FromEmailId { get; set; }
        public string ToEmailId { get; set; }
        public string Subject { get; set; }
        public string CursorName { get; set; }
        public virtual CompanyCompact Company { get; set; }
        public virtual TemplateTypeCompact TemplateType { get; set; }
    }
}

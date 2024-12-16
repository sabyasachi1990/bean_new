
using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.TemplateModule.Entities.Models
{
    public class GenericTemplate : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.Guid> TemplateTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TempletContent { get; set; }
        public Nullable<bool> IsSystem { get; set; }
        public Nullable<bool> IsFooterExist { get; set; }
        public Nullable<bool> IsHeaderExist { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }

        public string Conditions { get; set; }
        public string Category { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }

        public string TemplateType { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<bool> IsUsed { get; set; }
        public Nullable<short> Version { get; set; }
        public string FromEmailId { get; set; }

        public string CCEmailIds { get; set; }

        public string BCCEmailIds { get; set; }

        public string ToEmailId { get; set; }

        public string Subject { get; set; }
        public Nullable<bool> IsPartnerTemplate { get; set; }

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



       // public virtual Company Company { get; set; }


    }
}
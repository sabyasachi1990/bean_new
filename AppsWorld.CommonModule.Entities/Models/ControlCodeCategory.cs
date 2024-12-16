using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.Entities
{
    public partial class ControlCodeCategory : Entity
    {
        public ControlCodeCategory()
        {
            this.ControlCodes = new List<ControlCode>();
            
        }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string ControlCodeCategoryCode { get; set; }
        public string ControlCodeCategoryDescription { get; set; }
        public string DataType { get; set; }
        public string Format { get; set; }
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
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        public string ModuleNamesUsing { get; set; }
        public string DefaultValue { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<ControlCode> ControlCodes { get; set; }
       
    }
}

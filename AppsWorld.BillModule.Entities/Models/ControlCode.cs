using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
//using Newtonsoft.Json;using FrameWork;
//using Newtonsoft.Json.Converters;

namespace AppsWorld.BillModule.Entities
{
    
    public partial class ControlCode : Entity
    {
        public long Id { get; set; }

        public long ControlCategoryId { get; set; }

        [StringLength(100)]
        public string CodeKey { get; set; }

        [StringLength(100)]
        public string CodeValue { get; set; }

        [StringLength(10)]
        public string IsSystem { get; set; }

        public Nullable<int> RecOrder { get; set; }

        [StringLength(254)]
        public string Remarks { get; set; }

        [StringLength(254)]
        public string UserCreated { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        [StringLength(254)]
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

        public Nullable<bool> IsDefault { get; set; }

        [ForeignKey("ControlCategoryId")]
        public virtual ControlCodeCategory ControlCodeCategory { get; set; }
    }
}

using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    public partial class ControlCode : Entity
    {
        public long Id { get; set; }

        public long ControlCategoryId { get; set; }

        public string CodeKey { get; set; }

        public string CodeValue { get; set; }

        public string IsSystem { get; set; }

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

        public Nullable<bool> IsDefault { get; set; }

        [ForeignKey("ControlCategoryId")]
        public virtual ControlCodeCategory ControlCodeCategory { get; set; }
    }
}

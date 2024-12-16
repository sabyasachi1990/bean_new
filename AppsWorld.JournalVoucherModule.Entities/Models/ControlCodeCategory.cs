using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
//using Newtonsoft.Json;using FrameWork;
//using Newtonsoft.Json.Converters;

namespace AppsWorld.JournalVoucherModule.Entities
{
     
    public partial class ControlCodeCategory : Entity
    {
        public ControlCodeCategory()
        {
            this.ControlCodes = new List<ControlCode>();
        //    this.ControlCodeCategoryModules = new List<ControlCodeCategoryModule>();
        }

        public long Id { get; set; }

        public long CompanyId { get; set; }

        [Required]
        [StringLength(20)]
        public string ControlCodeCategoryCode { get; set; }

        [Required]
        [StringLength(100)]
        public string ControlCodeCategoryDescription { get; set; }

        [StringLength(10)]
        public string DataType { get; set; }

        [StringLength(20)]
        public string Format { get; set; }

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

        [StringLength(1000)]
        public string ModuleNamesUsing { get; set; }

        [StringLength(100)]
        public string DefaultValue { get; set; }

        //public virtual Company Company { get; set; }

        public virtual ICollection<ControlCode> ControlCodes { get; set; }

        //public virtual ICollection<ControlCodeCategoryModule> ControlCodeCategoryModules { get; set; }
    }
}

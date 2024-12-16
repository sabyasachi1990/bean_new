using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class Permission : Entity
    {
        public Permission()
        {
            //this.ModuleDetailPermissions = new List<ModuleDetailPermission>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public string FontAwesomeClass { get; set; }
        public Nullable<bool> IsFunctionOnly { get; set; }
        public int RecOrder { get; set; }
        public string Remarks { get; set; }

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
        //public virtual ICollection<ModuleDetailPermission> ModuleDetailPermissions { get; set; }
    }
}

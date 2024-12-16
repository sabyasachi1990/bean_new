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
    public partial class MultiCurrencySetting : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<bool> Revaluation { get; set; }
        AppsWorld.Framework.RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public AppsWorld.Framework.RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (AppsWorld.Framework.RecordStatusEnum)value; }
        }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [NotMapped]
        public bool IsEdit { get; set; }
        [NotMapped]
        public long? ModuleDetailId { get; set; }
        [NotMapped]
        public bool? IsRevalutionCoaPosted { get; set; }
    }
}

using AppsWorld.Framework;
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

namespace AppsWorld.PaymentModule.Entities
{
    public partial class CompanyFeature:Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.Guid FeatureId { get; set; }
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
        public string Remarks { get; set; }
        //public virtual Company Company { get; set; }
        public virtual Feature Feature { get; set; }
    }
}

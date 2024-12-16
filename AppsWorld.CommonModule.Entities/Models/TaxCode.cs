using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Entities
{
    public class TaxCode : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        public string TaxType { get; set; }
        public Nullable<double> TaxRate { get; set; }
        //public Nullable<int> RecOrder { get; set; }
        //public Nullable<short> Version { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public bool? IsApplicable { get; set; }
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
    }
}

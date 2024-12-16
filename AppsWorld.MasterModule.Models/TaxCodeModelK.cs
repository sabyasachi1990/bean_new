using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.MasterModule.Models
{
    public class TaxCodeModelK
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        public string AppliesTo { get; set; }
        public string TaxType { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        // public bool IsSystem { get; set; }
        //  public Nullable<int> RecOrder { get; set; }
        //  public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public string Applicable { get; set; }
        public string Status { get; set; }

        public bool? IsSeedData { get; set; }
        public string Jurisdiction { get; set; }
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum TaxcodeStatus
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
    }
}

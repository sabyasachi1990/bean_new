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

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class Feature:Entity
    {
        public Feature()
        {
            this.CompanyFeatures = new List<CompanyFeature>();
        }

        public System.Guid Id { get; set; }
        public Nullable<long> ModuleId { get; set; }
        public string Name { get; set; }
        public string VisibleStyle { get; set; }
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
        public virtual ICollection<CompanyFeature> CompanyFeatures { get; set; }
    }
}

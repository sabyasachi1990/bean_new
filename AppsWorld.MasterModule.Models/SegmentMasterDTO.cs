using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class SegmentMasterDTO
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        //public string SegmentMasterName { get; set; }
        //public string DetailMasterName { get; set; }
        //public string SegmentDetailName { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public bool IsSystem { get; set; }
        public Nullable<int> RecOrder { get; set; }
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
        public string UserCreated { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long? ModuleDetailId { get; set; }
        public List<SegmentDetailDTO> SegmentDetailsDTO { get; set; }

    }
}

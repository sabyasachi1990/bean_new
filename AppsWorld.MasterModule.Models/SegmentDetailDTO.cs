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
    public partial class SegmentDetailDTO 
    {
        public long Id { get; set; }
        public long SegmentMasterId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public long ParentId { get; set; }
        public bool IsSystem { get; set; }
        public Nullable<int> RecOrder { get; set; }
        [StringLength(256)]
        public string Remarks { get; set; }
        [StringLength(254)]
        public string UserCreated { get; set; }
        public System.DateTime CreatedDate { get; set; }
        [StringLength(254)]
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
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
        //public virtual Company Company { get; set; }
        //public List<SegmentDetail> SegmentDetails { get; set; }

        //[ForeignKey("MasterId")]
        //public virtual SegmentMaster SegmentMaster { get; set; }

        public Repository.Pattern.Infrastructure.ObjectState ObjectState { get; set; }
    }
}

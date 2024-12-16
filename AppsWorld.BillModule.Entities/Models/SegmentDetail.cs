using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
namespace AppsWorld.BillModule.Entities
{
    //public partial class SegmentDetail : Entity
    //{
    //    public long Id { get; set; }
    //    public long SegmentMasterId { get; set; }

    //    [Required]
    //    [StringLength(100)]
    //    public string Name { get; set; }
    //    public long ParentId { get; set; }
    //    public bool IsSystem { get; set; }
    //    public Nullable<int> RecOrder { get; set; }
    //    [StringLength(256)]
    //    public string Remarks { get; set; }
    //    [StringLength(254)]
    //    public string UserCreated { get; set; }
    //    public System.DateTime CreatedDate { get; set; }
    //    [StringLength(254)]
    //    public string  ModifiedBy { get; set; }
    //    public DateTime ModifiedDate { get; set; }
    //    public Nullable<short> Version { get; set; }

    //    RecordStatusEnum _status;
    //    [Required]
    //    [JsonConverter(typeof(StringEnumConverter))]
    //    [StatusValue]
    //    public RecordStatusEnum Status
    //    {
    //        get
    //        {
    //            return _status;
    //        }
    //        set { _status = (RecordStatusEnum)value; }
    //    }

    //    //public List<SegmentDetail> SegmentDetails { get; set; } 
    //    //public virtual Company Company { get; set; }

    //    //[ForeignKey("MasterId")]
    //    //public virtual SegmentMaster SegmentMaster { get; set; }
    //}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.Entities
{
    //public partial class SegmentDetail : Entity
    //{
    //    public long Id { get; set; }
    //    public long SegmentMasterId { get; set; }
    //    public string Name { get; set; }
    //    public Nullable<long> ParentId { get; set; }
    //    public bool IsSystem { get; set; }
    //    public Nullable<int> RecOrder { get; set; }
    //    public string Remarks { get; set; }
    //    public string UserCreated { get; set; }
    //    public Nullable<System.DateTime> CreatedDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public Nullable<System.DateTime> ModifiedDate { get; set; }
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
    //    public virtual SegmentMaster SegmentMaster { get; set; }
    //}
}
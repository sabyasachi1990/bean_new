using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.BankReconciliationModule.Entities;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.BankReconciliationModule.Entities
{
    //public partial class BankReconciliationSetting:Entity
    //{
    //    public long Id { get; set; }
    //    public long CompanyId { get; set; }
    //    public Nullable<System.DateTime> BankClearingDate { get; set; }
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
    //    public string UserCreated { get; set; }
    //    public Nullable<System.DateTime> CreatedDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public Nullable<System.DateTime> ModifiedDate { get; set; }
    //    public virtual Company Company { get; set; }
    //}
}

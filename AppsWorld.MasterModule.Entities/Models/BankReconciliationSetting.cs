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

namespace AppsWorld.MasterModule.Entities
{
    //public partial class BankReconciliationSetting:Entity
    //{
    //    public long Id { get; set; }
    //    public long CompanyId { get; set; }
    //    public Nullable<System.DateTime> BankClearingDate { get; set; }
    //    RecordStatusEnum _status;
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

    //    //public Nullable<int> Status { get; set; }
    //    public string UserCreated { get; set; }
    //    public Nullable<System.DateTime> CreatedDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public Nullable<System.DateTime> ModifiedDate { get; set; }
    //   // public virtual Company Company { get; set; }
    //}
}

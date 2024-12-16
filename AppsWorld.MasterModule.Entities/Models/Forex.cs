using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    //public partial class Forex:Entity
    //{
    //    public long Id { get; set; }
    //    public long CompanyId { get; set; }
    //    public string Type { get; set; }
    //    public System.DateTime DateFrom { get; set; }
    //    public System.DateTime Dateto { get; set; }
    //    public string Currency { get; set; }
    //    public decimal UnitPerUSD { get; set; }
    //    public decimal USDPerUnit { get; set; }
    //    public string Notes { get; set; }
    //    public Nullable<int> RecOrder { get; set; }
    //    public string Remarks { get; set; }
    //    public string UserCreated { get; set; }
    //    public Nullable<System.DateTime> CreatedDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public Nullable<System.DateTime> ModifiedDate { get; set; }
    //    public Nullable<short> Version { get; set; }
    //    public decimal? UnitPerCal { get; set; }
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

    //    string _unitPerUSDStr;
    //    [NotMapped]
    //    public string UnitPerUSDStr
    //    {
    //        get
    //        {
    //            _unitPerUSDStr = UnitPerUSD.ToString("0.0000000000");
    //            return _unitPerUSDStr;
    //        }
    //        set
    //        {
    //            decimal _unitPerUSD = 0;
    //            _unitPerUSDStr = value;
    //            decimal.TryParse(_unitPerUSDStr, out _unitPerUSD);
    //            UnitPerUSD = _unitPerUSD;
    //        }
    //    }
    //    [NotMapped]
    //    public bool IsWithinFinancialPeriod { get; set; }
    //    [NotMapped]
    //    public System.DateTime? GSTDateFrom { get; set; }
    //    [NotMapped]
    //    public System.DateTime? GSTDateTo { get; set; }
    //    [NotMapped]
    //    public decimal? GSTUnitPerUSD { get; set; }
    //    [NotMapped]
    //    public System.DateTime? BaseDateFrom { get; set; }
    //    [NotMapped]
    //    public System.DateTime? BaseDateTo { get; set; }
    //    [NotMapped]
    //    public decimal? BaseUnitPerUSD { get; set; }
    //    //   public virtual Company Company { get; set; }
    //}

}

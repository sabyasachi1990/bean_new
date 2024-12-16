using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;


namespace AppsWorld.JournalVoucherModule.Entities
{
    //public partial class Forex : Entity
    //{
    //    public long Id { get; set; }
    //    //public long CompanyId { get; set; }

    //    [Required]
    //    [StringLength(20)]
    //    public string Type { get; set; }
    //    public System.DateTime DateFrom { get; set; }
    //    public System.DateTime Dateto { get; set; }
    //    [Required]
    //    [StringLength(10)]
    //    public string Currency { get; set; }

        
    //    public decimal UnitPerUSD { get; set; }
    //    public decimal USDPerUnit { get; set; }
    //    public decimal? UnitPerCal { get; set; }
    //    public string Notes { get; set; }
    //    public Nullable<int> RecOrder { get; set; }
    //    [StringLength(256)]
    //    public string Remarks { get; set; }
    //    [StringLength(254)]
    //    public string UserCreated { get; set; }
    //    public Nullable<System.DateTime> CreatedDate { get; set; }
    //    [StringLength(254)]
    //    public string ModifiedBy { get; set; }
    //    public Nullable<System.DateTime> ModifiedDate { get; set; }
    //    public Nullable<short> Version { get; set; }
    

    //    [NotMapped]
    //    public bool IsWithinFinancialPeriod { get; set; }
        
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
    //}
}

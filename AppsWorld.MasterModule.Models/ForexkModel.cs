using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
	public partial class ForexkModel
    {
        
        public long Id { get; set; }
        public long CompanyId { get; set; }

		public System.DateTime DateFrom { get; set; }
		public System.DateTime Dateto { get; set; }
        public string Currency { get; set; }

       
        public decimal UnitPerUSD { get; set; }
        public decimal USDPerUnit { get; set; }
      
        public string GstStatus { get; set; }
        public string MultyCurrencyStatus { get; set; }
		[NotMapped]
		public bool IsWithinFinancialPeriod { get; set; }

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

        string _unitPerUSDStr;
        public string UnitPerUSDStr
        {
            get
            {
                _unitPerUSDStr = UnitPerUSD.ToString("0.0000000000");
                return _unitPerUSDStr;
            }

            set
            {
                decimal _unitPerUSD = 0;
                _unitPerUSDStr = value;
                decimal.TryParse(_unitPerUSDStr, out _unitPerUSD);
                UnitPerUSD = _unitPerUSD;
            }

        }
		public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

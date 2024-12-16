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

namespace AppsWorld.BillModule.Entities
{
    public class CommonForex : Entity
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Source { get; set; }
        public System.DateTime DateFrom { get; set; }
        public System.DateTime Dateto { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal? FromForexRate { get; set; }
        public decimal? ToForexRate { get; set; }

        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }

        RecordStatusEnum _status;
        [System.ComponentModel.DataAnnotations.Required]
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
    }
}

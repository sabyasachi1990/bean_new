using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class ScreenRecordsSave
    {
        public string Id { get; set; }
        public string FeatureId { get; set; }
        public string RecordId { get; set; }
        public string recordName { get; set; }
        public bool? isAdd { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public string CursorName { get; set; }
        public string ScreenName { get; set; }
        public string ReferenceId { get; set; }
        public long CompanyId { get; set; }
        public string OldFeatureId { get; set; }
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
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
    }
}

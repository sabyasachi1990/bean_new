using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
   public class InterCoClearingVM
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string InterCompanyType { get; set; }
        public bool? IsInterCompanyEnabled { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public List<LstClearingServiceCompany> LstInterCoClearing { get; set; }
        public List<LstClearingServiceCompany> LstClearingDetail { get; set; }
        public bool? ISIBActivated { get; set; }
    }
    public class LstClearingServiceCompany
    {
        public Guid Id { get; set; }
        public Guid InterCompanySettingId { get; set; }
        public long? ServiceEntityId { get; set; }
        public string ServiceEntityName { get; set; }
        public string SVCName { get; set; }
        public string RecordStatus { get; set; }
        public bool? IsMoved { get; set; }
        //public bool? COAStatus { get; set; }
        public int RecOrder { get; set; }
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
        RecordStatusEnum _cOAStatus;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum COAStatus
        {
            get
            {
                return _cOAStatus;
            }
            set { _cOAStatus = (RecordStatusEnum)value; }
        }
    }
}

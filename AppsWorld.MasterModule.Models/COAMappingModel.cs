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
    public class COAMappingModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public List<COAMappingDetailModel> COAMappingDetailsModel { get; set; }
        public List<COAMappingDetailModel> LstCOAMappingDetail { get; set; }
    }
    public class COAMappingDetailModel
    {
        public Guid Id { get; set; }
        public Guid COAMappingId { get; set; }
        public long? CustCOAId { get; set; }
        public long? VenCOAId { get; set; }
        public int? RecOrder { get; set; }
        public bool? IsDeleted { get; set; }
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
        public string RecordStatus { get; set; }
    }
}

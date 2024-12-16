using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
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
    public class ItemWFModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(20)]
     
        public long COAId { get; set; }
        public Nullable<long> DefaultTaxcodeId { get; set; }       
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [StringLength(254)]
       
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
        public bool? IsExternalData { get; set; }
     

        
    }
}

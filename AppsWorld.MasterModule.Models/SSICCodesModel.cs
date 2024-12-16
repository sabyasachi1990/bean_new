using AppsWorld.MasterModule.Entities.Models;
using System;
using System.Collections.Generic;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;

namespace AppsWorld.MasterModule.Models
{
    public class SSICCodesModel
    {
        public SSICCodesModel()
        {
            this.SSIICodes = new List<SSICCodes>();

        }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
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
        public DateTime? CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string IndustryName { get; set; }
        public List<SSICCodes> SSIICodes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using AppsWorld.Framework;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public partial class CompanyUser:Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public string UserIntial { get; set; }
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
        public string Salutation { get; set; }
        public string Remarks { get; set; }
        public System.Guid UserId { get; set; }
      

    }
}

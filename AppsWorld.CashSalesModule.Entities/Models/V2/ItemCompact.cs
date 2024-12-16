using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public partial class ItemCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(30)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(20)]

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

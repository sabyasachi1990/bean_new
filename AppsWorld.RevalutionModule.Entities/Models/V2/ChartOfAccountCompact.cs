using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;


namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class ChartOfAccountCompact : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsRealCOA { get; set; }
        public long AccountTypeId { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public int? IsRevaluation { get; set; }
        public bool? Revaluation { get; set; }
        public bool? IsBank { get; set; }
        public string Nature { get; set; }
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

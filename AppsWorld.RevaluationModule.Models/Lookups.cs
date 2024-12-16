using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;


namespace AppsWorld.RevaluationModule.Models
{
    public class Lookups<T>
    {
        public string TableName { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool? isGstActivated { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsRevaluation { get; set; }
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

    public class LookUpCompany<T>
    {
        public T Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool? IsBaseCompany { get; set; }
        public bool? isGstActivated { get; set; }
    }
}


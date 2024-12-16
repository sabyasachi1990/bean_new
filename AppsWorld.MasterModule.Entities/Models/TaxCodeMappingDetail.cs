using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.MasterModule.Entities
{
    public partial class TaxCodeMappingDetail : Entity
    {
        public Guid Id { get; set; }
        public Guid TaxCodeMappingId { get; set; }
        public long? CustTaxId { get; set; }
        public long? VenTaxId { get; set; }
        public string CustTaxCode { get; set; }
        public string VenTaxCode { get; set; }

        public int? RecOrder { get; set; }
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

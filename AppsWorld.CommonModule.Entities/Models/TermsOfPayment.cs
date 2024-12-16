using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;
namespace AppsWorld.CommonModule.Entities 
{
    public partial class TermsOfPayment : Entity
    {
        

        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public long CompanyId { get; set; }

        [StringLength(20)]
        public string TermsType { get; set; }

        public Nullable<double> TOPValue { get; set; }

        public Nullable<int> RecOrder { get; set; }

       
        public bool? IsVendor { get; set; }
        public bool? IsCustomer { get; set; }

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

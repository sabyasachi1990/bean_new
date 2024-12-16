using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class Company : Entity
    {

        public long Id { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string RegistrationNo { get; set; }
        public string Name { get; set; }
        public string BaseCurrency { get; set; }

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
        public string ShortName { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }

    }
}

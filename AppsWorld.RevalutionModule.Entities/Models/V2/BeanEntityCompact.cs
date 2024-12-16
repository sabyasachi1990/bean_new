using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class BeanEntityCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public decimal? CreditLimitValue { get; set; }

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

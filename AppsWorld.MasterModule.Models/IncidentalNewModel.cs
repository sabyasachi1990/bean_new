using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class IncidentalNewModel
    {
        public Guid Id { get; set; }
        public string IncidentalType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long? COAId { get; set; }
        public string COAName { get; set; }
        public long? DefaultTaxcodeId { get; set; }
        public long? DocumentId { get; set; }

        public long CompanyId { get; set; }
        public bool? IsIncidental { get; set; }

        public bool? IsExternalData { get; set; }

        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsAllowableNotAllowableActivated { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public bool? IsSaleItem { get; set; }
        public bool? IsPLAccount { get; set; }
        RecordStatusEnum _status;

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (RecordStatusEnum)value; }
        }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public FrameWork.LookUpCategory<string> IncidentalTypeLU { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string Notes { get; set; }

    }


}

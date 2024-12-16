using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class Revaluation : Entity
    {
        public Revaluation()
        {
            this.RevalutionDetails = new List<RevalutionDetail>();
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.DateTime> RevalutionDate { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string DocState { get; set; }
        public string SystemRefNo { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public bool? IsAllowableDisAllowable { get; set; }
        public bool? IsSegmentReporting { get; set; }
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
        public Nullable<decimal> ExchangeRate { get; set; }
        public long? ServiceCompanyId { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("RevalutionId")]
        public virtual List<RevalutionDetail> RevalutionDetails { get; set; }
    }
}

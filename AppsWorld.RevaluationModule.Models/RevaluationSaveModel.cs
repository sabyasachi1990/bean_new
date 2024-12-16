using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppsWorld.RevaluationModule.Models
{
    public class RevaluationSaveModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.DateTime> RevaluationDate { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string DocState { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public decimal? NetAmount { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public string Version { get; set; }
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
		public string DocType { get; set; }

		public List<RevaluationModel> RevaluationModels { get; set; }
    }
}
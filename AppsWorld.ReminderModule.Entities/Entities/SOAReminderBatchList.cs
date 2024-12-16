using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public partial class SOAReminderBatchList : Entity
    {
        public SOAReminderBatchList()
        {
            this.ReminderBatchListDetails = new List<SOAReminderBatchListDetails>();
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.Guid DocumentId { get; set; }
        public System.Guid TemplateId { get; set; }
        public string ReminderType { get; set; }
        public string Name { get; set; }
        public string Recipient { get; set; }
        public Nullable<System.DateTime> JobExecutedOn { get; set; }
        public string JobStatus { get; set; }
        public bool? IsDismiss { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        public virtual ICollection<SOAReminderBatchListDetails> ReminderBatchListDetails { get; set; }
    }
}

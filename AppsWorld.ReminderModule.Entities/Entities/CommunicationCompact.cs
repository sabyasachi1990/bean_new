using AppsWorld.Framework;
using FrameWork;
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
    public class CommunicationCompact : Entity
    {
        public System.Guid Id { get; set; }
        public string CommunicationType { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string SentBy { get; set; }
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (RecordStatusEnum)value; }
        }

        public Nullable<System.Guid> LeadId { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> TemplateId { get; set; }
        public string ReportPath { get; set; }
        public string Category { get; set; }
        public string TemplateName { get; set; }
        public string TemplateCode { get; set; }
        //   public string CommunicationValue { get; set; }
        public Nullable<System.Guid> InvoiceId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string AzurePath { get; set; }
        public string InvoiceNumber { get; set; }
        public string TemplateContent { get; set; }
        public string CCMail { get; set; }
        public string BCCMail { get; set; }
        public long CompanyId { get; set; }
    }
}
